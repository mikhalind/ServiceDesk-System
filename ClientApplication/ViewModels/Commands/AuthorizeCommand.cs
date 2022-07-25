using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApplication.ViewModels.Commands
{
    public class AuthorizeCommand : ICommand
    {
        public Action<object> AuthAction { get; private set; }

        public AuthorizeCommand(Action<object> _action) => AuthAction = _action;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => AuthAction.Invoke(parameter);

        public void OnExecutionChanged() => CanExecuteChanged.Invoke(this, new EventArgs());
    }
}
