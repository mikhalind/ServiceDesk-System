using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApplication.ViewModels.Commands
{
    public class CreateClaimCommand : ICommand
    {
        public Action<object> CreateAction { get; private set; }

        public CreateClaimCommand(Action<object> _action) => CreateAction = _action;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => CreateAction.Invoke(parameter);

        public void OnExecutionChanged() => CanExecuteChanged.Invoke(this, new EventArgs());
    }
}
