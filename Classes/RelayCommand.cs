using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrototypeDB.Classes
{
    public class RelayCommand : System.Windows.Input.ICommand
    {
#pragma warning disable IDE0044 // Readonly-Modifizierer hinzufügen
        private Action<object> execute;
#pragma warning restore IDE0044 // Readonly-Modifizierer hinzufügen
#pragma warning disable IDE0044 // Readonly-Modifizierer hinzufügen
        private Func<object, bool> canExecute;
#pragma warning restore IDE0044 // Readonly-Modifizierer hinzufügen

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
