using PrototypeDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeDB
{
    public class ReleaseTabViewModel : ViewModelBase, IView
    {
        private List<IView> _viewModels;

        public IView SelectedTabItemViewModel1 { get; set; }       
        public IView SelectedTabItemViewModel2 { get; set; }

        public List<IView> ViewModels
        {
            get
            {
                if (_viewModels == null)
                {
                    _viewModels = new List<IView>();
                }
                return _viewModels;
            }
        }


        public ReleaseTabViewModel()
        {
            ViewModels.Add(new ReleaseErstellenViewModel());
            ViewModels.Add(new ReleasesViewModel());

            SelectedTabItemViewModel1 = ViewModels[0];
            SelectedTabItemViewModel2 = ViewModels[1];
        }
    }
}
