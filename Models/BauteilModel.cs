using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeDB
{
    public class BauteilModel:ViewModelBase
    {
        private string _bedarf;

        public string BWNummer { get; set; }
        public string MaterialNummer { get; set; }
        public string BauteilBezeichnung { get; set; }
        public string Wert { get; set; }
        public string Bauform { get; set; }
        public string Spannung { get; set; }
        public string Leistung { get; set; }
        public string Toleranz { get; set; }
        public string Lebensdauer { get; set; }
        public string Bestand { get; set; }
        public string Bedarf
        {
            get { return _bedarf; }
            set { _bedarf = value; RaisePropertyChange("Bedarf"); }
        }


        public string MaterialStatus { get; set; }
        public string ZusatzInfo1 { get; set; }
        public string ZusatzInfo2 { get; set; }
    }
}
