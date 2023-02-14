using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using PrototypeDB.Classes;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Data;
using System.Windows;
using Microsoft.Win32;
using PrototypeDB.Interfaces;
using System.Reflection;

namespace PrototypeDB
{
    public class BauteilViewModel : ViewModelBase, IView
    {
        private string _dBPath = AppDomain.CurrentDomain.BaseDirectory;
        private List<string> _columns;
        private SqlConnection _connection = new SqlConnection();

        private bool _bauteilBezeichungCheck;
        private bool _wertCheck;
        private bool _bestandCheck;
        private int _selectedIndex;
        private string _textFilter;
        //private string _bauteilFilter;
        private string _id;
        private string _materialNummerEingabe;
        private string _bauteilBezeichnungEingabe;
        private string _wertEingabe;
        private string _leistungEingabe;
        private string _spannungEingabe;
        private string _toleranzEingabe;
        private string _temperaturKoeffizientEingabe;
        private string _lebensdauerEingabe;
        private string _bauformEingabe;
        private string _bestandEingabe;
        private string _bedarfEingabe;
        private string _materialStatus;
        private string _zusatzInfo1Eingabe;
        private string _zusatzInfo2Eingabe;
        private string _anzahlAusgabe;


        public bool BauteilBezeichnungCheck
        {
            get { return _bauteilBezeichungCheck;}
            set
            {
                _bauteilBezeichungCheck = value;
                RaisePropertyChange("BauteilBezeichnungCheck");
            }
        }

        public bool WertCheck
        {
            get { return _wertCheck; }
            set
            {
                _wertCheck = value;
                RaisePropertyChange("WertCheck");
            }
        }

        public bool BestandCheck
        {
            get { return _bestandCheck; }
            set
            {
                _bestandCheck = value;
                RaisePropertyChange("BestandCheck");
            }
        }

        public string Id
        {
            private get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChange("Id");
            }
        }

        public string MaterialNummerEingabe
        {
            get { return _materialNummerEingabe; }
            set
            {
                _materialNummerEingabe = value;
                RaisePropertyChange("MaterialNummerEingabe");
            }
        }

        public string BauteilBezeichnungEingabe
        {
            get { return _bauteilBezeichnungEingabe; }
            set
            {
                _bauteilBezeichnungEingabe = value;
                RaisePropertyChange("BauteilBezeichnungEingabe");
            }
        }

        public string WertEingabe
        {
            get { return _wertEingabe; }
            set
            {
                _wertEingabe = value;
                RaisePropertyChange("WertEingabe");
            }
        }

        public string LeistungEingabe
        {
            get { return _leistungEingabe; }
            set
            {
                _leistungEingabe = value;
                RaisePropertyChange("LeistungEingabe");
            }
        }

        public string SpannungEingabe
        {
            get { return _spannungEingabe; }
            set
            {
                _spannungEingabe = value;
                RaisePropertyChange("SpannungEingabe");
            }
        }

        public string ToleranzEingabe
        {
            get { return _toleranzEingabe; }
            set
            {
                _toleranzEingabe = value;
                RaisePropertyChange("ToleranzEingabe");
            }
        }

        public string TemperaturKoeffizientEingabe
        {
            get { return _temperaturKoeffizientEingabe; }
            set
            {
                _temperaturKoeffizientEingabe = value;
                RaisePropertyChange("TemperaturKoeffizientEingabe");
            }
        }

        public string LebensdauerEingabe
        {
            get { return _lebensdauerEingabe; }
            set
            {
                _lebensdauerEingabe = value;
                RaisePropertyChange("LebensdauerEingabe");
            }
        }

        public string BauformEingabe
        {
            get { return _bauformEingabe; }
            set
            {
                _bauformEingabe = value;
                RaisePropertyChange("BauformEingabe");
            }
        }

        public string BestandEingabe
        {
            get { return _bestandEingabe; }
            set
            {
                _bestandEingabe = value;
                RaisePropertyChange("BestandEingabe");
            }
        }

        public string BedarfEingabe
        {
            get { return _bedarfEingabe; }
            set
            {
                _bedarfEingabe = value;
                RaisePropertyChange("BedarfEingabe");
            }
        }

        public string MaterialStatus
        {
            get { return _materialStatus; }
            set
            {
                _materialStatus = value;
                RaisePropertyChange("MaterialStatus");
            }
        }

        public string ZusatzInfo1Eingabe
        {
            get { return _zusatzInfo1Eingabe; }
            set
            {
                _zusatzInfo1Eingabe = value;
                RaisePropertyChange("ZusatzInfo1Eingabe");
            }
        }

        public string ZusatzInfo2Eingabe
        {
            get { return _zusatzInfo2Eingabe; }
            set
            {
                _zusatzInfo2Eingabe = value;
                RaisePropertyChange("ZusatzInfo2Eingabe");
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

        public string TextFilter
        {
            get { return _textFilter; }
            set
            {
                RaisePropertyChange("TextFilter");
                _textFilter = value;
                FilterListItems(); }
        }


        //public string BauteilFilter
        //{
        //    get { return _bauteilFilter; }
        //    set
        //    {
        //        RaisePropertyChange("BauteilFilter");
        //        _bauteilFilter = value;
        //        FilterTextBoxUnitItems();
        //    }
        //}


        public int SelectedListIndex
        {
            get { return _selectedIndex; }
            set
            {
                RaisePropertyChange("SelectedIndex");
                _selectedIndex = value;
                SetDBTextBoxValues();
            }
        }

        public string UnitAuswahl { private get; set; }

        public bool MaterialNummerCheck { get; set; }
        public bool LeistungCheck { get; set; }
        public bool TemperaturKoeffizientCheck { get; set; }
        public bool ToleranzCheck { get; set; }

        public ICommand InsertCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand ReadFileButtonCommand { get; set; }

        public ObservableCollection<string> Units { private get; set; }
        public ObservableCollection<string> WiderstandUnits { get; set; }
        public ObservableCollection<string> InduktivitaetUnits { get; set; }
        public ObservableCollection<string> KapazitaetUnits { get; set; }
        public ObservableCollection<string> SpannungUnits { get; set; }
        public ObservableCollection<string> LeistungUnits { get; set; }
        public ObservableCollection<string> ToleranzUnits { get; set; }
        public ObservableCollection<BauteilModel> Bauteile { get; set; }
        public ObservableCollection<string> BauteilAuswahl { get; set; }

        public BauteilViewModel()
        {
            _connection.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={_dBPath}PrototypenDB.mdf;Integrated Security=True;Connect Timeout=30";


            MaterialNummerCheck = true;
            BauteilBezeichnungCheck = true;
            WertCheck = true;
            LeistungCheck = true;
            TemperaturKoeffizientCheck = true;
            ToleranzCheck = true;
            BestandCheck = true;

            Id = "";
            MaterialNummerEingabe = "";
            BauteilBezeichnungEingabe = "";
            WertEingabe = "";
            BauformEingabe = "";
            SpannungEingabe = "";
            LeistungEingabe = "";
            ToleranzEingabe = "";
            BestandEingabe = "";
            LebensdauerEingabe = "";
            BedarfEingabe = "";
            MaterialStatus = "";
            ZusatzInfo1Eingabe = "";
            ZusatzInfo2Eingabe = "";

            _columns = new List<string> (Enum.GetNames(typeof(SQLBauteilDBColumns)));

            //WiderstandUnits = new ObservableCollection<string>(Enum.GetNames(typeof(WiderstandDim)));
            //InduktivitaetUnits = new ObservableCollection<string>(Enum.GetNames(typeof(InduktivitaetDim)));
            //KapazitaetUnits = new ObservableCollection<string>(Enum.GetNames(typeof(KapazitaetDim)));
            //KapazitaetUnits = new ObservableCollection<string>(Enum.GetNames(typeof(SpannungDim)));
            //LeistungUnits = new ObservableCollection<string>(Enum.GetNames(typeof(LeistungDim)));
            //ToleranzUnits = new ObservableCollection<string>(Enum.GetNames(typeof(ToleranzDim)));
            Bauteile = new ObservableCollection<BauteilModel>();
            //BauteilAuswahl = new ObservableCollection<string>(Enum.GetNames(typeof(Bauteilart)));
            //BauteilFilter = BauteilAuswahl[0];


            InsertCommand = new RelayCommand(parameter => InsertCommandAction((object) parameter));
            DeleteCommand = new RelayCommand(parameter => DeleteCommandAction((object)parameter));
            UpdateCommand = new RelayCommand(parameter => UpdateCommandAction((object)parameter));
        }


        /// <summary>
        /// Aktualisierung der Listbox. Aufruf über Eventtrigger, wenn das Loaded-Event beendet ist.
        /// </summary>
        public void RefreshListbox()
        {
            Bauteile.Clear();
            FillListBox();
            UpdateBauteilAnzahlInfo();
        }

        private void UpdateBauteilAnzahlInfo() => AnzahlAusgabe = Convert.ToString(Bauteile.Count);

        /// <summary>
        /// Neuer Datensatz in die SQL-Datenbank
        /// </summary>
        /// <param name="parameter"></param>
        private void InsertCommandAction(object parameter)
        {
            if (CheckInput() == false) { return; }

            List<string> values = new List<string>() { MaterialNummerEingabe, BauteilBezeichnungEingabe, WertEingabe, BauformEingabe, SpannungEingabe, LeistungEingabe, ToleranzEingabe, LebensdauerEingabe, BestandEingabe, BedarfEingabe, MaterialStatus, ZusatzInfo1Eingabe, ZusatzInfo2Eingabe };

            Id = DBTableAdapter.DBInsert(_connection, true, "BAUTEILE", _columns, ref values);

            int row = DBTableAdapter.DBGetRowsCount(_connection, "BAUTEILE") - 1;
            DataRow dr = DBTableAdapter.DBGetRow(_connection, "BAUTEILE", row);
            NewDataRowToListBox(dr);

            UpdateBauteilAnzahlInfo();
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
            catch(Exception ex)
            {
                MessageBox.Show("Fehler in Modul BauteilViewModel\n" + ex.Message);
            }
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
        
            UpdateBauteilAnzahlInfo();
        }


        /// <summary>
        /// Filterfunktion für die Liste
        /// </summary>
        private void FilterListItems()
        {
            DataSet FilteredDataset;
            
            if(TextFilter == "")
            {
                Bauteile.Clear();
                FillListBox();
            }
            else
            {
                FilteredDataset = DBTableAdapter.DBFilterSearchValues(_connection, "BAUTEILE", Enum.GetNames(typeof(SQLBauteilDBColumns)).ToList(), TextFilter);

                if(FilteredDataset != null)
                {
                    Bauteile.Clear();

                    foreach (DataRow dr in FilteredDataset.Tables[0].Rows)
                    {
                        NewDataRowToListBox(dr);
                    }
                }
            }

            UpdateBauteilAnzahlInfo();
        }


        /// <summary>
        /// Einsetzen der Werte in die Maske je nach ausgewähltem Item in der Liste
        /// </summary>
        private void SetDBTextBoxValues()
        {
            if(SelectedListIndex == -1)
            {
                return;
            }

            BauteilModel SelectedBauteil = Bauteile[SelectedListIndex];

            Id = SelectedBauteil.BWNummer;
            MaterialNummerEingabe = SelectedBauteil.MaterialNummer;
            BauteilBezeichnungEingabe = SelectedBauteil.BauteilBezeichnung;
            WertEingabe = SelectedBauteil.Wert;
            BauformEingabe = SelectedBauteil.Bauform;
            SpannungEingabe = SelectedBauteil.Spannung;
            LeistungEingabe = SelectedBauteil.Leistung;
            ToleranzEingabe = SelectedBauteil.Toleranz;
            BestandEingabe = SelectedBauteil.Bestand;
            LebensdauerEingabe = SelectedBauteil.Lebensdauer;
            BedarfEingabe = SelectedBauteil.Bedarf;
            MaterialStatus = SelectedBauteil.ZusatzInfo1;
            ZusatzInfo1Eingabe = SelectedBauteil.ZusatzInfo1;
            ZusatzInfo2Eingabe = SelectedBauteil.ZusatzInfo2;
        }


        /// <summary>
        /// Löschen eines ausgewälten List-Items
        /// </summary>
        /// <param name="parameter"></param>
        private void DeleteCommandAction(object parameter)
        {
            if(SelectedListIndex == -1)
            {
                return;
            }

            BauteilModel DeleteBauteil = Bauteile[SelectedListIndex];
            //DBTableAdapter.Delete(_connection, "BAUTEILE", Convert.ToInt32(DeleteBauteil.BWNummer));
            int ID = Convert.ToInt32(DeleteBauteil.BWNummer);

            if (DBTableAdapter.Delete(_connection, "BAUTEILE", "BAUID", ref ID))
            {
                Bauteile.RemoveAt(SelectedListIndex);

                Id = "";
                MaterialNummerEingabe = "";
                BauteilBezeichnungEingabe = "";
                WertEingabe = "";
                BauformEingabe = "";
                SpannungEingabe = "";
                LeistungEingabe = "";
                ToleranzEingabe = "";
                LebensdauerEingabe = "";
                BestandEingabe = "";
                BedarfEingabe = "";
                MaterialStatus = "";
                ZusatzInfo1Eingabe = "";
                ZusatzInfo2Eingabe = "";

                UpdateBauteilAnzahlInfo();
            }
        }


        private void UpdateCommandAction(object parameter)
        {
            if (SelectedListIndex == -1)
            {
                return;
            }

            int currentIndex = SelectedListIndex;
            BauteilModel UpdateBauteil = Bauteile[currentIndex];

            List<string> values = new List<string>() { MaterialNummerEingabe,
                                                    BauteilBezeichnungEingabe,
                                                    WertEingabe,
                                                    BauformEingabe,
                                                    SpannungEingabe,
                                                    LeistungEingabe,
                                                    ToleranzEingabe,
                                                    LebensdauerEingabe,
                                                    BestandEingabe,
                                                    BedarfEingabe,
                                                    MaterialStatus,
                                                    ZusatzInfo1Eingabe,
                                                    ZusatzInfo2Eingabe };

            if (DBTableAdapter.Update(_connection, "BAUTEILE", Convert.ToInt32(UpdateBauteil.BWNummer), _columns, values))
            {
                Bauteile.Clear();
                FillListBox();
                SelectedListIndex = currentIndex;
            }
        }

        /// <summary>
        /// Eingabeprüfung der Werte in der Maske
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            bool checkIO = true;
            //Pflichtfeld, muss befüllt werden
            if (BauteilBezeichnungEingabe == "")
            {
                BauteilBezeichnungCheck = false;
                checkIO = false;
            }
            else
            {
                BauteilBezeichnungCheck = true;
            }

            if (BestandEingabe != "")
            {
                if (!int.TryParse(BestandEingabe, out int result1))
                {
                    BestandCheck = false;
                    checkIO = false;
                }
                else
                {
                    BestandCheck = true;
                }
            }

            //Pflichtfeld, muss befüllt werden
            if (WertEingabe == "")
            {
                WertCheck = false;
                checkIO = false;
            }
            else
            {
                WertCheck = true;
            }

            //if (ToleranzEingabe != "")
            //{
            //    if (!double.TryParse(ToleranzEingabe, out double result3))
            //    {
            //        ToleranzCheck = false;
            //        RaisePropertyChange("ToleranzCheck");
            //        checkIO = false;
            //    }
            //    else
            //    {
            //        WertCheck = true;
            //        RaisePropertyChange("ToleranzCheck");
            //    }
            //}

            //if (LeistungEingabe != "")
            //{
            //    if (!double.TryParse(LeistungEingabe, out double result4))
            //    {
            //        LeistungCheck = false;
            //        RaisePropertyChange("LeistungCheck");
            //        checkIO = false;
            //    }
            //    else
            //    {
            //        LeistungCheck = true;
            //        RaisePropertyChange("LeistungCheck");
            //    }
            //}

            //    if (TemperaturKoeffizientEingabe != "")
            //    {
            //        if (!double.TryParse(TemperaturKoeffizientEingabe, out double result5))
            //        {
            //            TemperaturKoeffizientCheck = true;
            //            RaisePropertyChange("TemperaturKoeffizientCheck");
            //            checkIO = false;
            //        }
            //        else
            //        {
            //            TemperaturKoeffizientCheck = true;
            //            RaisePropertyChange("TemperaturKoeffizientCheck");
            //        }
            //    }

            return checkIO;
        }


        /// <summary>
        /// Auswahl eines Wertes für die Comboboxes in der Eingabemaske
        /// </summary>
        //private void FilterTextBoxUnitItems()
        //{

        //    if (BauteilFilter == Enum.GetName(typeof(Bauteilart), 0))
        //    {
        //        Units = WiderstandUnits;
        //        RaisePropertyChange("Units");

        //        UnitAuswahl = WiderstandUnits[0];
        //        RaisePropertyChange("UnitAuswahl");

        //    }
        //    if (BauteilFilter == Enum.GetName(typeof(Bauteilart), 1))
        //    {
        //        Units = InduktivitaetUnits;
        //        RaisePropertyChange("Units");

        //        UnitAuswahl = InduktivitaetUnits[0];
        //        RaisePropertyChange("UnitAuswahl");
        //    }
        //    if (BauteilFilter == Enum.GetName(typeof(Bauteilart), 2))
        //    {
        //        Units = KapazitaetUnits;
        //        RaisePropertyChange("Units");

        //        UnitAuswahl = KapazitaetUnits[0];
        //        RaisePropertyChange("UnitAuswahl");

        //    }
        //    if (BauteilFilter == Enum.GetName(typeof(Bauteilart), 3))
        //    {
        //        Units = InduktivitaetUnits;
        //        RaisePropertyChange("Units");

        //        UnitAuswahl = InduktivitaetUnits[0];
        //        RaisePropertyChange("UnitAuswahl");
        //    }

        //}
    }

}

