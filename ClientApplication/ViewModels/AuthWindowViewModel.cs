using ClientApplication.Helpers;
using ClientApplication.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClientApplication.ViewModels
{
    public class AuthWindowViewModel : BaseViewModel, IClosable
    {
        private string email;
        private string password;
        private AuthorizeCommand authorizeCmd;
        private DBService.EmployeeEntity authorizedEmployee;
        private bool succeeded;
        private Action close;

        public DBService.EmployeeEntity AuthorizedEmployee
        {
            get { return authorizedEmployee; }
            set { SetProperty(ref authorizedEmployee, value); }
        }

        public AuthorizeCommand AuthorizeCommand
        {
            get { return authorizeCmd; }
            set { SetProperty(ref authorizeCmd, value); }
        }

        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public AuthWindowViewModel()
        {
            IsSucceeded = false;
            Email = "VolkovII@ant.com";
            authorizeCmd = new AuthorizeCommand((obj) => Auth(obj));
        }

        public Action Close
        {
            get { return close; }
            set { SetProperty(ref close, value); }
        }

        public bool IsSucceeded
        {
            get { return succeeded; }
            set { SetProperty(ref succeeded, value); }
        }

        public void CloseWindow() => Close?.Invoke();

        public void Auth(object param)
        { 
            DBService.DatabaseServiceClient client = new DBService.DatabaseServiceClient();
            Password = (param as PasswordBox).Password;

            if (Email == null || Email == string.Empty) { MessageBox.Show("Введите логин"); return; }
            if (Password == null || Password == string.Empty) { MessageBox.Show("Введите пароль"); return; }

            try
            {
                var result = client.Authorize(Email, HashingHelper.CreateMD5(Password));
                if (result == null)
                {
                    MessageBox.Show("Failed");
                    WindowsContainer.Display<MessageWindowViewModel>(obj => { });
                    IsSucceeded = false;
                }
                else
                {
                    AuthorizedEmployee = result;
                    MessageBox.Show($"Успешно авторизован: {result.LastName} {result.FirstName[0]}. {result.MiddleName[0]}.");
                    IsSucceeded = true;
                    CloseWindow();
                }
            }
            catch (Exception)
            {
                WindowsContainer.Display<MessageWindowViewModel>(obj => { });
            }            
        }
    }
}
