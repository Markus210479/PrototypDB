using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PrototypeDB
{
    public class DBComboBoxControl : ComboBox
    {
        public static readonly DependencyProperty ValueCheckIOProperty = DependencyProperty.Register("ValueCheckIO", typeof(bool), typeof(DBComboBoxControl), new PropertyMetadata(true));

        static DBComboBoxControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DBComboBoxControl), new FrameworkPropertyMetadata(typeof(DBComboBoxControl)));        

        public bool ValueCheckIO
        {
            get { return (bool)GetValue(ValueCheckIOProperty); }
            set { SetValue(ValueCheckIOProperty, value); }
        }
    }
}
