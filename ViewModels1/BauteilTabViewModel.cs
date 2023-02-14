using PrototypeDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PrototypeDB
{
    public class BauteilTabViewModel : ViewModelBase, IView
    {
        //private IView _selectedTabItemViewModel1;
        //private IView _selectedTabItemViewModel2;

        private List<IView> _viewModels;

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

        public IView SelectedTabItemViewModel1 { get; set; }
        //{
        //    get
        //    {
        //        return _selectedTabItemViewModel1;
        //    }
        //    set
        //    {
        //        _selectedTabItemViewModel1 = value;
        //        RaisePropertyChange("SelectedTabItemViewModel1");
        //    }
        //}

        public IView SelectedTabItemViewModel2 { get; set; }
        //{
        //    get
        //    {
        //        return _selectedTabItemViewModel2;
        //    }
        //    set
        //    {
        //        _selectedTabItemViewModel2 = value;
        //        RaisePropertyChange("SelectedTabItemViewModel2");
        //    }
        //}

        //private void ChangeViewModel(IView viewModel)
        //{
        //    if (!ViewModels.Contains(viewModel))
        //    {
        //        ViewModels.Add(viewModel);
        //    }
        //    SelectedViewModel = ViewModels.FirstOrDefault(vm => vm == viewModel);

        //}

        //private void ChangeToBauteilView(object obj) => ChangeViewModel(ViewModels[0]);
        ////private void ChangeToReadFileView(object obj) => ChangeViewModel(ViewModels[1]);
        //private void ChangeToReleasesView(object obj) => ChangeViewModel(ViewModels[1]);

        public BauteilTabViewModel()
        {
            ViewModels.Add(new BauteilViewModel());
            ViewModels.Add(new ReadFileViewModel());


            ////BauteilButtonCommand = new RelayCommand(parameter => BauteilButtonCommandAction((object)parameter));
            //////ReadFileButtonCommand = new RelayCommand(parameter => ReadFileButtonCommandAction((object)parameter));
            ////ReleasesButtonCommand = new RelayCommand(parameter => ReleasesButtonCommandAction((object)parameter));


            SelectedTabItemViewModel1 = ViewModels[0];
            SelectedTabItemViewModel2 = ViewModels[1];


            //Mediator.Subscribe("BauteilListe", ChangeToBauteilView);
            ////Mediator.Subscribe("ReadFile", ChangeToReadFileView);
            //Mediator.Subscribe("Releases", ChangeToReleasesView);
        }

       
    }
}
