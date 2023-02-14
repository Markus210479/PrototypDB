using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrototypeDB
{
    public class DBTextBoxWithComboBoxControl : DBTextBoxControl
    {
        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(ObservableCollection<string>), typeof(DBTextBoxWithComboBoxControl), new PropertyMetadata());
        public static readonly DependencyProperty UnitAuswahlProperty = DependencyProperty.Register("UnitAuswahl", typeof(string), typeof(DBTextBoxWithComboBoxControl), new PropertyMetadata());

        static DBTextBoxWithComboBoxControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DBTextBoxWithComboBoxControl), new FrameworkPropertyMetadata(typeof(DBTextBoxWithComboBoxControl)));

        public ObservableCollection<string> Unit
        {
            get { return (ObservableCollection<string>)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public string UnitAuswahl
        {
            get { return (string)GetValue(UnitAuswahlProperty); }
            set { SetValue(UnitAuswahlProperty, value); }
        }      
    }
}
