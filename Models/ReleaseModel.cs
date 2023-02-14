using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeDB
{
    public class ReleaseModel : ModelBase
    {
        private string _relID;
        private string _releaseName;
        private string _releaseNummer;
        private string _layout;
        private string _releaseInfo;

        public string RelID
        {
            get { return _relID; }
            set
            {
                _relID = value;
                RaisePropertyChange("RelID");
            }
        }

        public string ReleaseName
        {
            get { return _releaseName; }
            set
            {
                _releaseName = value;
                RaisePropertyChange("ReleaseName");
            }
        }

        public string ReleaseNummer
        {
            get { return _releaseNummer; }
            set
            {
                _releaseNummer = value;
                RaisePropertyChange("ReleaseNummer");
            }
        }

        public string ReleaseInfo
        {
            get { return _releaseInfo; }
            set
            {
                _releaseInfo = value;
                RaisePropertyChange("ReleaseInfo");
            }
        }

        public string Layout
        {
            get { return _layout; }
            set
            {
                _layout = value;
                RaisePropertyChange("Layout");
            }
        }                        
              
        public ReleaseModel()
        {
            RelID = "";
            ReleaseName = "";
            ReleaseNummer = "";
            Layout = "";
            ReleaseInfo = "";
        }

        public ReleaseModel(string relID, string releaseName, string releaseNummer, string layout, string releaseInfo)
        {
            RelID = relID;
            ReleaseName = releaseName;
            ReleaseNummer = releaseNummer;
            Layout = layout;
            ReleaseInfo = releaseInfo;
        }
    }
}
