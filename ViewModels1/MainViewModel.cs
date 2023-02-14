using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrototypeDB.Classes;
using PrototypeDB.Interfaces;
using System.Windows.Input;
using System.Data.Common;
using System.Windows;

namespace PrototypeDB
{
    public class MainViewModel : ViewModelBase
    {
        private IView _selectedViewModel;
        private List<IView> _viewModels;

        public ICommand BauteilButtonCommand { get; set; }
        public ICommand ReleasesButtonCommand { get; set; }

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
        
        public IView SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                RaisePropertyChange("SelectedViewModel");
            }
        }

        private void ChangeViewModel(IView viewModel)
        {
            if (!ViewModels.Contains(viewModel))
            {
                ViewModels.Add(viewModel);
            }
            SelectedViewModel = ViewModels.FirstOrDefault(vm => vm == viewModel);

        }

        private void ChangeToBauteilView(object obj) => ChangeViewModel(ViewModels[0]);
        //private void ChangeToReadFileView(object obj) => ChangeViewModel(ViewModels[1]);
        private void ChangeToReleasesView(object obj) => ChangeViewModel(ViewModels[1]);

        private void BauteilButtonCommandAction(object parameter) => Mediator.Notify("BauteilListe", "");
        //private void ReadFileButtonCommandAction(object parameter) => Mediator.Notify("ReadFile", "");
        private void ReleasesButtonCommandAction(object parameter) => Mediator.Notify("Releases", "");


        public MainViewModel()
        {
            ViewModels.Add(new BauteilTabViewModel());
            //ViewModels.Add(new ReadFileViewModel());
            ViewModels.Add(new ReleaseTabViewModel());


            BauteilButtonCommand = new RelayCommand(parameter => BauteilButtonCommandAction((object)parameter));
            //ReadFileButtonCommand = new RelayCommand(parameter => ReadFileButtonCommandAction((object)parameter));
            ReleasesButtonCommand = new RelayCommand(parameter => ReleasesButtonCommandAction((object)parameter));


            //SelectedViewModel = ViewModels[0];
            
            Mediator.Subscribe("BauteilListe", ChangeToBauteilView);
            //Mediator.Subscribe("ReadFile", ChangeToReadFileView);
            Mediator.Subscribe("Releases", ChangeToReleasesView);

            Mediator.Notify("BauteilListe", "");
        }
    }
}
