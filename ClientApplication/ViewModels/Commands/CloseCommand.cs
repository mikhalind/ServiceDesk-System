using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApplication.ViewModels.Commands
{
    public class CloseCommand : ICommand
    {
        public Action CloseAction { get; private set; }

        public CloseCommand(Action _action) => CloseAction = _action;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => CloseAction?.Invoke();

        public void OnExecutionChanged() => CanExecuteChanged.Invoke(this, new EventArgs());
    }
}
