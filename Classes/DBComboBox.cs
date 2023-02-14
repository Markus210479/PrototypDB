using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PrototypeDB.Classes;

namespace PrototypeDB
{
    public class DBComboBox : ModelBase
    {
        private int _comboBoxSelectedIndex;
        private bool _comboBoxCheck;

        public bool ComboBoxCheck
        {
            get { return _comboBoxCheck; }
            set
            {
                _comboBoxCheck = value;
                RaisePropertyChange("ComboBoxCheck");
            }
        }

        public int ComboBoxSelectedIndex
        {   get { return _comboBoxSelectedIndex; }
            set
            {
                _comboBoxSelectedIndex = value;
                RaisePropertyChange("ComboBoxSelectedIndex");
            }
        }       

        public string ComboBoxSelectedItem { get; }

        public DBComboBox(int comboBoxSelectedIndex, bool comboBoxCheck)
        {
            ComboBoxSelectedIndex = comboBoxSelectedIndex;
            ComboBoxCheck = comboBoxCheck;
        }

        public DBComboBox()
        {
            ComboBoxSelectedIndex = 0;
            ComboBoxCheck = true;          
        }

  
    }
}
