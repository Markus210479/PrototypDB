using PrototypeDB.Classes;
using PrototypeDB.Interfaces;
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

namespace PrototypeDB
{
    public class ReleasesViewModel : ViewModelBase, IView
    {
        private string _dBPath = AppDomain.CurrentDomain.BaseDirectory;
        private string _dBConnection;
        private int _relSelectedListIndex;
        private bool _releaseNameCheck;
        private string _releaseNameEingabe;
        private string _releaseNummerEingabe;
        private string _releaseInfoEingabe;
        private string _layoutEingabe;
        private string _anzahlAusgabe;
        private SqlConnection _junctionConnection;
        private SqlConnection _releasesConnection;
        private SqlConnection _bauteileConnection;

        public ObservableCollection<ReleaseModel> Releases { get; set; }
        public ObservableCollection<BauteilModel> ReleaseBauteile { get; set; }

        public ICommand ReleaseSelChangeCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }


        public int RelSelectedListIndex
        {
            get { return _relSelectedListIndex; }
            set
            {
                _relSelectedListIndex = value;
                RaisePropertyChange("RelSelectedListIndex");
            }
        }

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

        public ReleasesViewModel()
        {
            _dBConnection = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={_dBPath}PrototypenDB.mdf;Integrated Security=True;Connect Timeout=30";
            _junctionConnection = new SqlConnection(_dBConnection);
            _releasesConnection = new SqlConnection(_dBConnection);
            _bauteileConnection = new SqlConnection(_dBConnection);

            Releases = new ObservableCollection<ReleaseModel>();
            ReleaseBauteile = new ObservableCollection<BauteilModel>();

            ReleaseNameEingabe = "";
            ReleaseNummerEingabe = "";
            ReleaseInfoEingabe = "";
            ReleaseNameCheck = true;

            ReleaseSelChangeCommand = new RelayCommand(parameter => RelSelChangeCommandAction((object)parameter));
            DeleteCommand = new RelayCommand(parameter => DeleteCommandAction((object)parameter));
            UpdateCommand = new RelayCommand(parameter => UpdateCommandAction((object)parameter));

        }


        /// <summary>
        /// Suchen der BAUIds zu den RELIDs in der Junction-Tabelle und Aufruf zum Befüllen der BauteilTabelle
        /// </summary>
        /// <param name="parameter"></param>
        private void RelSelChangeCommandAction(object parameter)
        {
            if(RelSelectedListIndex == -1)
            {
                return;
            }

            int searchID = Convert.ToInt32(((ReleaseModel)parameter).RelID);

            ReleaseBauteile.Clear();
            try
            {                
                DataSet Dataset = DBTableAdapter.GetDataSet($"SELECT * FROM JUNC_REL_BAU WHERE RELID = {searchID}", "JUNC_REL_BAU", _junctionConnection);

                //using (SqlCommand junctionTableCommand = new SqlCommand($"SELECT * FROM JUNC_REL_BAU WHERE RELID={searchID}", _junctionConnection))
                //{
                //    _junctionConnection.Open();
                //    SqlDataAdapter junctionTableAdapter = new SqlDataAdapter(junctionTableCommand);
                //    DataSet junctionTableDataset = new DataSet();
                //    junctionTableAdapter.Fill(junctionTableDataset, "JUNC_REL_BAU");

                    foreach (DataRow dr in Dataset.Tables["JUNC_REL_BAU"].Rows)
                    //foreach (DataRow dr in junctionTableDataset.Tables["JUNC_REL_BAU"].Rows)
                    {
                        FillReleaseBauteile(dr);
                    }

                //    junctionTableAdapter.Dispose();
                //    junctionTableDataset.Dispose();
                //    _junctionConnection.Close();
                //}

                SetReleaseTextBoxValues();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fehler in Modul ReleaseViewModel " + ex.Message);
            }
        }


        /// <summary>
        /// Befüllen der Bauteilliste für das gewählte Release
        /// </summary>
        private void FillReleaseBauteile(DataRow dr)
        {
            try
            {
                DataRow bauteilDataRow, junctionDataRow;
                DataSet bauteilDataset = DBTableAdapter.GetDataSet($"SELECT * FROM BAUTEILE WHERE BAUID={dr["BAUID"].ToString()}", "BAUTEILE", _bauteileConnection);
                junctionDataRow = dr;
                //using (SqlCommand searchReleaseBauteileCommand = new SqlCommand($"SELECT * FROM BAUTEILE WHERE BAUID={dr["BAUID"].ToString()}", _bauteileConnection))
                {
                    //_bauteileConnection.Open();
                    //SqlDataAdapter bauteileTableAdapter = new SqlDataAdapter(searchReleaseBauteileCommand);
                    //DataSet bauteileDataset = new DataSet();
                    //bauteileTableAdapter.Fill(bauteileDataset, "BAUTEILE");
                    bauteilDataRow = bauteilDataset.Tables["BAUTEILE"].Rows[0];

                    ReleaseBauteile.Add(new BauteilModel()
                    {
                        BWNummer = bauteilDataRow["BAUID"].ToString(),
                        MaterialNummer = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 0).ToString()].ToString(),
                        BauteilBezeichnung = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 1).ToString()].ToString(),
                        Wert = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 2).ToString()].ToString(),
                        Bauform = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 3).ToString()].ToString(),
                        Spannung = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 4).ToString()].ToString(),
                        Leistung = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 5).ToString()].ToString(),
                        Toleranz = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 6).ToString()].ToString(),
                        Lebensdauer = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 7).ToString()].ToString(),
                        Bestand = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 8).ToString()].ToString(),
                        //Bedarf = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 9).ToString()].ToString(),
                        Bedarf = junctionDataRow[Enum.GetName(typeof(SQLJunc_Rel_BauDBColumns), 2).ToString()].ToString(),

                        MaterialStatus = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 10).ToString()].ToString(),
                        ZusatzInfo1 = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 11).ToString()].ToString(),
                        ZusatzInfo2 = bauteilDataRow[Enum.GetName(typeof(SQLBauteilDBColumns), 12).ToString()].ToString()
                    });

                    //bauteileTableAdapter.Dispose();
                    //bauteileDataset.Dispose();
                    //_bauteileConnection.Close();

                    UpdateAnzahlAusgabe();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fehler in Modul ReleaseViewModel " + ex.Message);
            }
        }


        private void UpdateCommandAction(object parameter)
        {
            if (RelSelectedListIndex == -1)
            {
                return;
            }

            int currentIndex = RelSelectedListIndex;

            try
            {
                ReleaseModel UpdateBauteil = Releases[currentIndex];

                List<string> values = new List<string>() { ReleaseNameEingabe, ReleaseNummerEingabe, LayoutEingabe, ReleaseInfoEingabe };
                List<string> columns = new List<string>(Enum.GetNames(typeof(SQLReleasesDBColumns)));

                if (DBTableAdapter.Update(_releasesConnection, "RELEASES", Convert.ToInt32(UpdateBauteil.RelID), columns, values))
                {
                    DataSet Dataset = DBTableAdapter.GetDataSet($"SELECT * FROM RELEASES WHERE RELID = {Convert.ToInt32(UpdateBauteil.RelID)}", "RELEASES", _releasesConnection);                   

                    DataRow dr = Dataset.Tables["RELEASES"].Rows[0];

                    Releases[currentIndex].ReleaseName = dr[1].ToString();
                    Releases[currentIndex].ReleaseNummer = dr[2].ToString();
                    Releases[currentIndex].Layout = dr[3].ToString();
                    Releases[currentIndex].ReleaseInfo = dr[4].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler in Modul ReleasesViewModel\n" + ex.Message);
            }

            //Releases.Clear();
            //FillReleasesListBox();
            RelSelectedListIndex = currentIndex;
        }


        private void SetReleaseTextBoxValues()
        {
            if (RelSelectedListIndex == -1)
            {
                return;
            }

            ReleaseModel SelectedBauteil = Releases[RelSelectedListIndex];

            ReleaseNameEingabe = SelectedBauteil.ReleaseName;
            ReleaseNummerEingabe = SelectedBauteil.ReleaseNummer;
            ReleaseInfoEingabe = SelectedBauteil.ReleaseInfo;
            LayoutEingabe = SelectedBauteil.Layout;
        }

        private void DeleteCommandAction(object parameter)
        {
            if (RelSelectedListIndex == -1)
            {
                return;
            }

            try
            {               
                int RelID = Convert.ToInt32(Releases[RelSelectedListIndex].RelID);
                DataSet junctionDataset = DBTableAdapter.GetDataSet($"SELECT * FROM JUNC_REL_BAU WHERE RELID = {RelID}", "JUNC_REL_BAU", _junctionConnection);


                if (DBTableAdapter.Delete(_junctionConnection, "JUNC_REL_BAU", "RELID", ref RelID))                
                {
                    if (DBTableAdapter.Delete(_releasesConnection, "RELEASES", "RELID", ref RelID))
                    {
                        DataSet bauteileDataset;
                        DataRow bauteilDataRow;
                        string neuerBedarf = string.Empty;
                        foreach(DataRow dr in junctionDataset.Tables["JUNC_REL_BAU"].Rows)
                        {
                            bauteileDataset = DBTableAdapter.GetDataSet($"SELECT * FROM BAUTEILE WHERE BAUID = {dr["BAUID"]}", "BAUTEILE", _junctionConnection);

                            bauteilDataRow = bauteileDataset.Tables["BAUTEILE"].Rows[0];
                            neuerBedarf = Convert.ToString(Convert.ToInt32(bauteilDataRow["BEDARF"].ToString()) - Convert.ToInt32(dr["BEDARF"].ToString()));

                            DBTableAdapter.Update(_bauteileConnection, "BAUTEILE", Convert.ToInt32(dr["BAUID"]), new List<string>() { Enum.GetName(typeof(SQLBauteilDBColumns), 9).ToString() }, new List<string>() { neuerBedarf });
                        }

                        Releases.RemoveAt(RelSelectedListIndex);
                        ReleaseBauteile.Clear();
                        ReleaseNameEingabe = "";
                        ReleaseNummerEingabe = "";
                        LayoutEingabe = "";
                        ReleaseInfoEingabe = "";
                        UpdateAnzahlAusgabe();
                        //RefreshReleasesListbox();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler in Modul ReleaseViewModel " + ex.Message);
            }
        }

        private void UpdateAnzahlAusgabe()
        {
            int count = 0;
            foreach (BauteilModel BT in ReleaseBauteile)
            {
                count += Convert.ToInt32(BT.Bedarf);
            }
            AnzahlAusgabe = Convert.ToString(count);
            //AnzahlAusgabe = Convert.ToString(ReleaseBauteile.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshReleasesListbox()
        {
            RelSelectedListIndex = -1;
            ReleaseBauteile.Clear();
            Releases.Clear();
            FillReleasesListBox();
            UpdateAnzahlAusgabe();
        }


        private void FillReleasesListBox()
        {
            DataSet Dataset = DBTableAdapter.GetDataSet("SELECT * FROM RELEASES", "RELEASES", _releasesConnection);
            try
            {
                foreach (DataRow dr in Dataset.Tables["RELEASES"].Rows)
                {
                    Releases.Add(new ReleaseModel() //Property Bauteile Binding to ListBox Itemsource
                    {
                        RelID = dr[0].ToString(),
                        ReleaseName = dr[1].ToString(),
                        ReleaseNummer = dr[2].ToString(),
                        Layout = dr[3].ToString(),
                        ReleaseInfo = dr[4].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler in Modul ReleaseViewModel\n" + ex.Message);
            }
        }       
    }
}
