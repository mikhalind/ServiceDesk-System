using ClientApplication.ViewModels.Commands;
using System;

namespace ClientApplication.ViewModels
{
    public class MessageWindowViewModel : BaseViewModel, IClosable
    {
        private string title;
        private string message;
        private CloseCommand closeWindowCommand;      

        public string Title
        { 
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public CloseCommand CloseWindowCommand
        {
            get => closeWindowCommand;
            set => SetProperty(ref closeWindowCommand, value);
        }

        public Action Close { get; set; }

        public MessageWindowViewModel()
        {
            Title = "Whoops!";
            Message = "If you can read this, something went definitely wrong =)";
            CloseWindowCommand = new CloseCommand(() => { Close?.Invoke(); });
        }
    }
}
