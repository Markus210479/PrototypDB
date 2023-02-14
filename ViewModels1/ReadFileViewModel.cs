using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using PrototypeDB.Interfaces;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Input;
using PrototypeDB.Classes;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;

namespace PrototypeDB
{
    public class ReadFileViewModel : ViewModelBase, IView
    {
        private string _fileName;
        Excel.Application _objExcel;
        Excel.Workbook _wbExcel;
        Excel.Worksheet _wsExcel;

        private string _auswahlBereichEingabe;
        private string _dBPath = AppDomain.CurrentDomain.BaseDirectory;
        private string _anzahlAusgabe;
        private List<string> _columns;
        private SqlConnection _connection = new SqlConnection();

        public ObservableCollection<string> ColumnMapping { get; set; }
        public ObservableCollection<ImportEintrag> Bauteile { get; set; }

        public ICommand DateiAuswahlButtonCommand { get; set; }
        public ICommand DateiImportButtonCommand { get; set; }       
                                     
        private readonly Dictionary<string, DBComboBox> _comboBoxDict;

        public DBComboBox MappingComboBox0 { get; set; }
        public DBComboBox MappingComboBox1 { get; set; }
        public DBComboBox MappingComboBox2 { get; set; }
        public DBComboBox MappingComboBox3 { get; set; }
        public DBComboBox MappingComboBox4 { get; set; }
        public DBComboBox MappingComboBox5 { get; set; }
        public DBComboBox MappingComboBox6 { get; set; }
        public DBComboBox MappingComboBox7 { get; set; }
        public DBComboBox MappingComboBox8 { get; set; }
        public DBComboBox MappingComboBox9 { get; set; }
        public DBComboBox MappingComboBox10 { get; set; }
        public DBComboBox MappingComboBox11 { get; set; }
        public DBComboBox MappingComboBox12 { get; set; }      

        public string AuswahlBereichEingabe
        {
            get { return _auswahlBereichEingabe; }
            set
            {
                RaisePropertyChange("AuswahlBereichEingabe");
                _auswahlBereichEingabe = value;
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


        /// <summary>
        /// Initializes a new instance of the <see cref="ReadFileViewModel"/> class.
        /// </summary>
        public ReadFileViewModel()
        {
            _connection.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={_dBPath}PrototypenDB.mdf;Integrated Security=True;Connect Timeout=30";


            Bauteile = new ObservableCollection<ImportEintrag>();
            ColumnMapping = new ObservableCollection<string>(Enum.GetNames(typeof(BauteilViewColumns)));
            ColumnMapping.Add("NONE");

            MappingComboBox0 = new DBComboBox(0, true);
            MappingComboBox1 = new DBComboBox(1, true);
            MappingComboBox2 = new DBComboBox(2, true);
            MappingComboBox3 = new DBComboBox(3, true);
            MappingComboBox4 = new DBComboBox(4, true);
            MappingComboBox5 = new DBComboBox(5, true);
            MappingComboBox6 = new DBComboBox(6, true);
            MappingComboBox7 = new DBComboBox(7, true);
            MappingComboBox8 = new DBComboBox(8, true);
            MappingComboBox9 = new DBComboBox(9, true);
            MappingComboBox10 = new DBComboBox(10, true);
            MappingComboBox11 = new DBComboBox(11, true);
            MappingComboBox12 = new DBComboBox(12, true);
            
            _comboBoxDict = new Dictionary<string, DBComboBox>()
            {
                { "MappingComboBox0", MappingComboBox0 },
                { "MappingComboBox1", MappingComboBox1 },
                { "MappingComboBox2", MappingComboBox2 },
                { "MappingComboBox3", MappingComboBox3 },
                { "MappingComboBox4", MappingComboBox4 },
                { "MappingComboBox5", MappingComboBox5 },
                { "MappingComboBox6", MappingComboBox6 },
                { "MappingComboBox7", MappingComboBox7 },
                { "MappingComboBox8", MappingComboBox8 },
                { "MappingComboBox9", MappingComboBox9 },
                { "MappingComboBox10", MappingComboBox10 },
                { "MappingComboBox11", MappingComboBox11 },
                { "MappingComboBox12", MappingComboBox12 }
            };

            _columns = new List<string>(Enum.GetNames(typeof(SQLBauteilDBColumns)));

            DateiAuswahlButtonCommand = new RelayCommand(parameter => DateiAuswahlButtonCommandAction((object)parameter));
            DateiImportButtonCommand = new RelayCommand(parameter => DateiImportButtonCommandAction((object)parameter));
        }


        private bool CheckComboBoxIndex()
        {
            bool checkIO = true;
            DBComboBox ComboBoxItem;
            Type DBComboBoxType;

            foreach (KeyValuePair<string, DBComboBox> item in _comboBoxDict)
            {
                DBComboBoxType = item.Value.GetType();
                ComboBoxItem = item.Value; 
                DBComboBoxType.GetProperty("ComboBoxCheck").SetValue(ComboBoxItem, true);
            }

            for (int k = 0; k < _comboBoxDict.Count; k++)
            {
                DBComboBoxType = _comboBoxDict[$"MappingComboBox{k}"].GetType();

                if (_comboBoxDict[$"MappingComboBox{k}"].ComboBoxSelectedIndex != 13)
                {
                    for (int i = k + 1; i < _comboBoxDict.Count; i++)
                    {
                        if(_comboBoxDict[$"MappingComboBox{k}"].ComboBoxSelectedIndex == _comboBoxDict[$"MappingComboBox{i}"].ComboBoxSelectedIndex)
                        {
                            ComboBoxItem = _comboBoxDict[$"MappingComboBox{k}"];
                            DBComboBoxType.GetProperty("ComboBoxCheck").SetValue(ComboBoxItem, false);
                            ComboBoxItem = _comboBoxDict[$"MappingComboBox{i}"];
                            DBComboBoxType.GetProperty("ComboBoxCheck").SetValue(ComboBoxItem, false);

                            checkIO = false;
                        }
                    }
                }              
            }

            return checkIO;
        }


        private void DateiImportButtonCommandAction(object parameter)
        {
            if(CheckComboBoxIndex() == false)
            {
                return;
            }

            int count = 0;
            string[] EintraegeArr = { "", "", "", "", "", "", "", "", "", "", "", "", ""};

            try
            {
                foreach(ImportEintrag Eintrag in Bauteile)
                {

                    for (int i = 0; i < EintraegeArr.Length; i++)
                    {
                        if (_comboBoxDict[$"MappingComboBox{i}"].ComboBoxSelectedIndex != 13)
                        {
                            EintraegeArr[_comboBoxDict[$"MappingComboBox{i}"].ComboBoxSelectedIndex] = Eintrag.GetType().GetProperties()[i].GetValue(Eintrag).ToString();
                        }
                    }

                    List<string> EintraegeList = EintraegeArr.ToList();
                    count += Convert.ToInt32(DBTableAdapter.DBInsert(_connection, false, "BAUTEILE", _columns, ref EintraegeList));
              
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fehler beim Datenimport\n" + ex.Message);
            }
            finally
            {
                string messageString = count == 1 ? "Datensatz" : "Datensätze";
                MessageBox.Show($"Es wurden {count} {messageString} importiert.");
            }



        }

        private void DateiAuswahlButtonCommandAction(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                _fileName = openFileDialog.FileName;
                ReadDataFile(_fileName);
            }
        }


        private void ReadDataFile(string file)
        {
            try
            {
                _objExcel = new Excel.Application();
                _objExcel.Visible = true;
                _wbExcel = _objExcel.Workbooks.Open(file);
                _wsExcel = _wbExcel.ActiveSheet;
                _wsExcel.SelectionChange += Ws_SelectionChange;               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler in ReadFileViewModel beim Öffnen/Bearbeiten der Datei!\n" + ex.Message);
                _wbExcel.Close();
            }

        }


        private async void Ws_SelectionChange(Excel.Range Target)
        {
            try
            {
                AuswahlBereichEingabe = Target.Address.ToString();
               
                for (int row = Target.Row; row < Target.Row + Target.Rows.Count; row++)
                {
                    await Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => { NewDataRowToListBox(Target, row); }));
                }

                AnzahlAusgabe = Convert.ToString(Target.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler Modul ReadFileModule\n" + ex.Message);
            }
        }


        private void NewDataRowToListBox(Excel.Range range, int row)
        {
            ImportEintrag importEintrag = new ImportEintrag();
            int propertyIndex = 0;

            for(int col = range.Column; col < range.Column + range.Columns.Count; col++)
            {

                string importString = _wsExcel.Cells[row, col].Value == null ? "" : Convert.ToString(_wsExcel.Cells[row, col].Value);

                importEintrag.GetType().GetProperties()[propertyIndex].SetValue(importEintrag, importString);

                propertyIndex++;

                if (propertyIndex >= importEintrag.GetType().GetProperties().Count())
                {
                    break;
                }
            }
         

            while(propertyIndex < importEintrag.GetType().GetProperties().Count())
            {
                importEintrag.GetType().GetProperties()[propertyIndex].SetValue(importEintrag,"");
                propertyIndex++;                
            }
            
            Bauteile.Add(importEintrag);     
        }
    }
}
