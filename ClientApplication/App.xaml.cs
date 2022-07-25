using ClientApplication.Helpers;
using ClientApplication.ViewModels;
using ClientApplication.Views;
using System.Windows;

namespace ClientApplication
{
    public partial class App : Application
    {
        public App()
        {
            WindowsContainer.RegisterDialog<AuthWindowViewModel, AuthWindow>();
            WindowsContainer.RegisterDialog<MessageWindowViewModel, MessageWindow>();
            WindowsContainer.RegisterDialog<MainWindowViewModel, MainWindow>();
        }
    }
}
