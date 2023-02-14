using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PrototypeDB.Classes;
using PrototypeDB.Interfaces;

namespace PrototypeDB
{
    public class ReleaseErstellenViewModel : ViewModelBase, IView
    {
        private string _dBPath = AppDomain.CurrentDomain.BaseDirectory;
        private SqlConnection _connection = new SqlConnection();
        private bool _releaseNameCheck;
        private string _releaseNameEingabe;
        private string _releaseNummerEingabe;
        private string _releaseInfoEingabe;
        private string _layoutEingabe;
        private string _anzahlAusgabe;

        public ReleaseModel Release { get; set; }
        public ObservableCollection<BauteilModel> Bauteile { get; set; }
        public ObservableCollection<BauteilModel> ReleaseBauteile { get; set; }

        public string ReleaseNameEingabe
        {
            get { return _releaseNameEingabe; }
            set
            {
                _releaseNameEingabe = value;
                RaisePropertyChange("ReleaseNameEingabe");
            }
        }

        public string ReleaseNummerEingabe
        {
            get { return _releaseNummerEingabe; }
            set
            {
                _releaseNummerEingabe = value;
                RaisePropertyChange("ReleaseNummerEingabe");
            }
        }

        public string ReleaseInfoEingabe
        {
            get { return _releaseInfoEingabe; }
            set
            {
                _releaseInfoEingabe = value;
                RaisePropertyChange("ReleaseInfoEingabe");
            }
        }

        public bool ReleaseNameCheck
        {
            get { return _releaseNameCheck; }
            set
            {
                _releaseNameCheck = value;
                RaisePropertyChange("ReleaseNameCheck");
            }
        }

        public string LayoutEingabe
        {
            get { return _layoutEingabe; }
            set
            {
                _layoutEingabe = value;
                RaisePropertyChange("LayoutEingabe");
            }
        }

        public string AnzahlAusgabe
        {
            get { return _anzahlAusgabe; }
            set
            {
                _anzahlAusgabe = value;
                RaisePropertyChange("AnzahlAusgabe");
            }
        }

        public ICommand BTButtonClkCommand { get; set; }
        public ICommand RelBTButtonClickCommand { get; set; }
        public ICommand ReleaseErstellenCommand { get; set; }


        public ReleaseErstellenViewModel()
        {
            _connection.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={_dBPath}PrototypenDB.mdf;Integrated Security=True;Connect Timeout=30";

            Bauteile = new ObservableCollection<BauteilModel>();
            ReleaseBauteile = new ObservableCollection<BauteilModel>();

            BTButtonClkCommand = new RelayCommand(parameter => BTButtonClkCommandAction((object)parameter));
            RelBTButtonClickCommand = new RelayCommand(parameter => RelBTButtonClickCommandAction((object)parameter));
            ReleaseErstellenCommand = new RelayCommand(parameter => ReleaseErstellenCommandAction((object)parameter));


            ReleaseNameEingabe = "";
            ReleaseNummerEingabe = "";
            ReleaseInfoEingabe = "";
            ReleaseNameCheck = true;
        }


        /// <summary>
        /// Aktualisierung der Listbox. Aufruf über Eventtrigger, wenn das Loaded-Event beendet ist.
        /// </summary>
        public void RefreshListbox()
        {
            Bauteile.Clear();
            FillListBox();
            UpdateAnzahlAusgabe();
        }

        private void BTButtonClkCommandAction(object parameter)
        {
            bool isAvailable = false;
            foreach(BauteilModel BTElement in ReleaseBauteile)
            {
                if (((BauteilModel)parameter).MaterialNummer == BTElement.MaterialNummer)
                {
                    int bedarf = BTElement.Bedarf == "" ? 0 : Convert.ToInt32(BTElement.Bedarf) + 1;
                    BTElement.Bedarf = bedarf.ToString();

                    isAvailable = true;
                    break;
                }
            }

            if(!isAvailable)
            {
                ReleaseBauteile.Add(new BauteilModel()
                {
                    BWNummer = ((BauteilModel)parameter).BWNummer,
                    MaterialNummer = ((BauteilModel)parameter).MaterialNummer,
                    BauteilBezeichnung = ((BauteilModel)parameter).BauteilBezeichnung,
                    Wert = ((BauteilModel)parameter).Wert,
                    Bauform = ((BauteilModel)parameter).Bauform,
                    Spannung = ((BauteilModel)parameter).Spannung,
                    Leistung = ((BauteilModel)parameter).Leistung,
                    Toleranz = ((BauteilModel)parameter).Toleranz,
                    Lebensdauer = ((BauteilModel)parameter).Lebensdauer,
                    Bestand = ((BauteilModel)parameter).Bestand,
                    Bedarf = "1",
                    MaterialStatus = ((BauteilModel)parameter).MaterialStatus,
                    ZusatzInfo1 = ((BauteilModel)parameter).ZusatzInfo1,
                    ZusatzInfo2 = ((BauteilModel)parameter).ZusatzInfo2
                });
            }            
            
            UpdateAnzahlAusgabe();
        }


        private void RelBTButtonClickCommandAction(object parameter)
        {
            if (Convert.ToInt32(((BauteilModel)parameter).Bedarf) > 1)
            {
                int bedarf = Convert.ToInt32(((BauteilModel)parameter).Bedarf);
                bedarf -= 1;
                ((BauteilModel)parameter).Bedarf = bedarf.ToString();               
            }
            else
            {
                ReleaseBauteile.Remove((BauteilModel)parameter);
            }

            UpdateAnzahlAusgabe();
        }

        private void UpdateAnzahlAusgabe()
        {
            int count = 0;
            foreach (BauteilModel BT in ReleaseBauteile)
            {
                count += Convert.ToInt32(BT.Bedarf);
            }
            AnzahlAusgabe = Convert.ToString(count);
        }


        /// <summary>
        /// Erstellen eines Release und Herstellung der m:n-Beziehung Release - Bauteile
        /// Anpassung der BauteilBedarfe
        /// </summary>
        /// <param name="parameter"></param>
        private void ReleaseErstellenCommandAction(object parameter)
        {
            
            if (ReleaseBauteile.Count == 0) { return; }
            if (CheckInput() == false) { return; }

            try
            {
                Release = new ReleaseModel("", ReleaseNameEingabe, ReleaseNummerEingabe, LayoutEingabe, ReleaseInfoEingabe);

                //Werte in die Releasedatenbank einfügen
                List<string> releaseValues = new List<string>() { ReleaseNameEingabe, ReleaseNummerEingabe, LayoutEingabe, ReleaseInfoEingabe };
                Release.RelID = DBTableAdapter.DBInsert(_connection, true, "RELEASES", Enum.GetNames(typeof(SQLReleasesDBColumns)).ToList(), ref releaseValues);
            
                //int row = DBTableAdapter.DBGetRowsCount(_connection, "RELEASES") - 1;
                //DataRow dr = DBTableAdapter.DBGetRow(_connection, "RELEASES", row);

              

                //Werte in die Junction-Tabelle für die Release-Bauteile-Verknüpfung einfügen sowie die Anzahl des jeweiligen pro Release verwendeten Bauteils
                List<int> juncRelBauValues = new List<int>();
                juncRelBauValues.Add(Convert.ToInt32(Release.RelID));
                foreach(BauteilModel ReleaseBT in ReleaseBauteile)
                {
                    juncRelBauValues.Add(Convert.ToInt32(ReleaseBT.BWNummer));
                    juncRelBauValues.Add(Convert.ToInt32(ReleaseBT.Bedarf));

                    string ID1 = DBTableAdapter.DBInsert(_connection, true, "JUNC_REL_BAU", Enum.GetNames(typeof(SQLJunc_Rel_BauDBColumns)).ToList(), ref juncRelBauValues);

                    juncRelBauValues.RemoveAt(2);
                    juncRelBauValues.RemoveAt(1);
                }

                if (Convert.ToInt32(Release.RelID) > 0)
                {
                    //Anpassung des Bauteilbedarfs in der Bauteil-Tabelle
                    DataSet Dataset;
                    DataRow dr;
                    foreach (BauteilModel RelBauteil in ReleaseBauteile)
                    {
                        Dataset = DBTableAdapter.GetDataSet($"SELECT * FROM BAUTEILE WHERE BAUID = {Convert.ToInt32(RelBauteil.BWNummer)}", "BAUTEILE", _connection);

                        dr = Dataset.Tables["BAUTEILE"].Rows[0];
                        int bedarf = dr["BEDARF"].ToString() == "" ? Convert.ToInt32(RelBauteil.Bedarf) : Convert.ToInt32(dr["BEDARF"].ToString()) + Convert.ToInt32(RelBauteil.Bedarf);
                        DBTableAdapter.Update(_connection, "BAUTEILE", Convert.ToInt32(RelBauteil.BWNummer), new List<string>() { "BEDARF" }, new List<string>() { Convert.ToString(bedarf) });
                    }

               
                    ReleaseNameEingabe = "";
                    ReleaseNummerEingabe = "";
                    ReleaseInfoEingabe = "";
                    LayoutEingabe = "";
                    ReleaseBauteile.Clear();
                    UpdateAnzahlAusgabe();

                    MessageBox.Show($"Neues Release mit ID {Release.RelID} wurde erstellt.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fehler in Modul ReleaseErstellenViewModel\n" + ex.Message);
            }            
        }

        /// <summary>
        /// Prüfung Eingabe eines Releasenamens
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            bool check = true;

            if (ReleaseNameEingabe == "")
            {
                ReleaseNameCheck = false;
                check = false;
            }
            else
            {
                ReleaseNameCheck = true;
            }

            return check;
        }


        /// <summary>
        /// initiales Befüllen der ListBox mit werten aus der SQL-Tabelle
        /// </summary>
        private void FillListBox()
        {
            DataSet Dataset = DBTableAdapter.GetDataSet("SELECT * FROM BAUTEILE", "BAUTEILE", _connection);                           
            foreach (DataRow dr in Dataset.Tables["BAUTEILE"].Rows)
            {
                NewDataRowToListBox(dr);
            }                          
        }

        /// <summary>
        /// Befüllen der ListBox mit einem Datensatz aus der SQL-Tabelle
        /// </summary>
        /// <param name="dr"></param>
        private void NewDataRowToListBox(DataRow dr)
        {
            try
            {
                Bauteile.Add(new BauteilModel() //Property Bauteile Binding to ListBox Itemsource
                {
                    BWNummer = dr["BAUID"].ToString(),
                    MaterialNummer = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 0).ToString()].ToString(),
                    BauteilBezeichnung = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 1).ToString()].ToString(),
                    Wert = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 2).ToString()].ToString(),
                    Bauform = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 3).ToString()].ToString(),
                    Spannung = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 4).ToString()].ToString(),
                    Leistung = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 5).ToString()].ToString(),
                    Toleranz = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 6).ToString()].ToString(),
                    Lebensdauer = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 7).ToString()].ToString(),
                    Bestand = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 8).ToString()].ToString(),
                    Bedarf = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 9).ToString()].ToString(),
                    MaterialStatus = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 10).ToString()].ToString(),
                    ZusatzInfo1 = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 11).ToString()].ToString(),
                    ZusatzInfo2 = dr[Enum.GetName(typeof(SQLBauteilDBColumns), 12).ToString()].ToString()
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler in Modul BauteilViewModel\n" + ex.Message);
            }
        }

      
    }
}
