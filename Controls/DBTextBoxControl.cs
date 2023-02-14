using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PrototypeDB
{
    public class DBTextBoxControl : TextBox
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(DBTextBoxControl), new PropertyMetadata());
        public static readonly DependencyProperty ValueCheckIOProperty = DependencyProperty.Register("ValueCheckIO", typeof(bool), typeof(DBTextBoxControl), new PropertyMetadata(true));

        static DBTextBoxControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DBTextBoxControl), new FrameworkPropertyMetadata(typeof(DBTextBoxControl)));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public bool ValueCheckIO
        {
            get { return (bool)GetValue(ValueCheckIOProperty); }
            set { SetValue(ValueCheckIOProperty, value); }
        }
    }
}
