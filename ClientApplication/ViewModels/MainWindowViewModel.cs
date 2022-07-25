using ClientApplication.DBService;
using ClientApplication.Helpers;
using ClientApplication.ViewModels.Commands;
using ClientApplication.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ClientApplication.ViewModels
{
    public class InfoConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            new Tuple<object, object>(values[0], values[1]);
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class PasswordConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            new List<byte[]>() { HashingHelper.CreateMD5(values[0].ToString()),
                                 HashingHelper.CreateMD5(values[1].ToString()),
                                 HashingHelper.CreateMD5(values[2].ToString()) };

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class MainWindowViewModel : BaseViewModel, IClosable
    {
        private ObservableCollection<DBService.ClaimEntity> claims;

        public RelayCommand TreeNodeSelected { get; private set; }
        public RelayCommand ApproveCommand{ get; private set; }
        public RelayCommand SetExecutorCommand { get; private set; }
        public RelayCommand AcceptCommand { get; private set; }
        public RelayCommand DeclineCommand { get; private set; }
        public RelayCommand FinishCommand { get; private set; }
        public RelayCommand ConfirmCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand UpdatePersonalInfoCommand { get; private set; }
        public RelayCommand UpdatePasswordCommand { get; private set; }
        public RelayCommand UpdateEmailCommand { get; private set; }

        public List<string> Languages { get; set; }

        private DBService.EmployeeEntity currentEmployee;
        private DBService.ClaimEntity currentClaim;

        public Action Close { get; set; }

        public void CloseWindow() => Close?.Invoke();

        public List<DBService.DepartmentEntity> Departments { get; set; }
        public List<DBService.PositionEntity> Positions { get; set; }
        public List<DBService.StatusEntity> Statuses { get; set; }

        public string CurrentDepartment { get; set; }
        public string CurrentPosition { get; set; }

        public DBService.ClaimEntity CurrentClaim
        {
            get => currentClaim;
            set => SetProperty(ref currentClaim, value);
        }

        public ObservableCollection<DBService.ClaimEntity> Claims
        {
            get => claims;
            set => SetProperty(ref claims, value);
        }

        public DBService.EmployeeEntity CurrentEmployee
        {
            get => currentEmployee;
            set => SetProperty(ref currentEmployee, value);
        }

        private bool canExecuteMethod(object parameter) => true;

        private void ExecuteTreeNodeSelected(object parameter)
        {
            TreeViewItem selectedNode = (TreeViewItem)parameter;
            DBService.DatabaseServiceClient client = new DBService.DatabaseServiceClient();
            List<DBService.StatusEntity> statuses = client.GetStatuses().ToList();
            IEnumerable<ClaimEntity> collection = null;
            switch (selectedNode.Name)
            {
                case ("AllClaimsItem"):
                    collection = client.GetAllClaims(CurrentEmployee, null);
                    break;
                case ("AllOpenedClaimsItem"):
                    collection = client.GetAllClaims(CurrentEmployee, statuses.FirstOrDefault(i => i.Code == 1));
                    break;
                case ("AllApprovedClaimsItem"):
                    collection = client.GetAllClaims(CurrentEmployee, statuses.FirstOrDefault(i => i.Code == 2));
                    break;
                case ("AllSentClaimsItem"):
                    collection = client.GetAllClaims(CurrentEmployee, statuses.FirstOrDefault(i => i.Code == 3));
                    break;
                case ("AllInProgressClaimsItem"):
                    collection = client.GetAllClaims(CurrentEmployee, statuses.FirstOrDefault(i => i.Code == 4));
                    break;
                case ("AllFinishedClaimsItem"):
                    collection = client.GetAllClaims(CurrentEmployee, statuses.FirstOrDefault(i => i.Code == 5));
                    break;
                case ("AllAcceptedClaimsItem"):
                    collection = client.GetAllClaims(CurrentEmployee, statuses.FirstOrDefault(i => i.Code == 6));
                    break;
                case ("MyClaimsItem"):
                    collection = client.GetEmployeesClaims(CurrentEmployee);
                    break;
                case ("ArchivedClaimsItem"):
                    collection = client.GetAllClaims(CurrentEmployee, statuses.FirstOrDefault(i => i.Code == 7));
                    break;
                default:
                    break;
            }

            Claims.Clear();
            foreach (var item in collection) Claims.Add(item);

            client.Close();
            return;
        }       

        public MainWindowViewModel()
        {
            WindowsContainer.Display<AuthWindowViewModel>(obj =>
            {
                AuthWindowViewModel authVM = (AuthWindowViewModel)obj;

                // completely closes application if authorization has been cancelled
                if (!authVM.IsSucceeded) Application.Current.Shutdown();
                else CurrentEmployee = authVM.AuthorizedEmployee;
            });

            Languages = new List<string>() { "Русский", "English", "Deutsch" };

            TreeNodeSelected = new RelayCommand(ExecuteTreeNodeSelected, canExecuteMethod);
            ApproveCommand = new RelayCommand(ExecuteApproveCommand, CanExecuteApproveCommand);
            SetExecutorCommand = new RelayCommand(ExecuteSetExecutorCommand, CanExecuteSetExecutorCommand);
            AcceptCommand = new RelayCommand(ExecuteAcceptCommand, CanExecuteAcceptCommand);
            DeclineCommand = new RelayCommand(ExecuteDeclineCommand, CanExecuteConfirmCommand);
            FinishCommand = new RelayCommand(ExecuteFinishCommand, CanExecuteFinishCommand);
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand, CanExecuteConfirmCommand);
            CloseCommand = new RelayCommand(ExecuteCloseCommand, CanExecuteCloseCommand);
            UpdatePersonalInfoCommand = new RelayCommand(ExecuteUpdatePersonalInfoCommand, CanExecuteUpdatePersonalInfoCommand);
            UpdatePasswordCommand = new RelayCommand(ExecuteUpdatePasswordCommand, CanExecuteUpdatePasswordCommand);
            UpdateEmailCommand = new RelayCommand(ExecuteUpdateEmailCommand, CanExecuteUpdateEmailCommand);

            DatabaseServiceClient client = new DatabaseServiceClient();

            Departments = client.GetDepartments().ToList();
            Positions = client.GetPositions().ToList();
            Statuses = client.GetStatuses().ToList();

            Claims = new ObservableCollection<DBService.ClaimEntity>();
            client.Close();

            CurrentDepartment = Departments.FirstOrDefault(i => i.ID == CurrentEmployee.Department).Name;
            CurrentPosition = Positions.FirstOrDefault(i => i.ID == CurrentEmployee.Position).Name;
        }

        private void ExecuteUpdatePasswordCommand(object obj)
        {
            var list = (List<byte[]>)obj;
            if (list[0].SequenceEqual(list[1]) == false) 
            { 
                MessageBox.Show("Введенные пароли отличаются", "Ошибка");
                return; 
            }
            DatabaseServiceClient client = new DatabaseServiceClient();
            client.UpdatePassword(CurrentEmployee.TableNumber, list[0], list[2]);
            client.Close();
        }

        private void ExecuteUpdateEmailCommand(object obj)
        {
            DatabaseServiceClient client = new DatabaseServiceClient();
            client.UpdateEmail(CurrentEmployee.TableNumber, (string)obj);
            client.Close();
        }

        private bool CanExecuteUpdatePersonalInfoCommand(object arg) => true;
        private bool CanExecuteUpdateEmailCommand(object arg) => true;
        private bool CanExecuteUpdatePasswordCommand(object arg) => true;

        private void ExecuteUpdatePersonalInfoCommand(object obj)
        {
            string phone = (string)((Tuple<object, object>)obj).Item1;
            if (phone.Any(ch => Char.IsDigit(ch) == false) ||
                phone.Length != 11 ||
                phone[0] == '0')
                MessageBox.Show("Неверный телефон");
            decimal phone_formatted = Int32.Parse(phone);

            string address = (string)((Tuple<object, object>)obj).Item2;
            DatabaseServiceClient client = new DatabaseServiceClient();
            client.UpdateAddress(CurrentEmployee.TableNumber, address);
            client.UpdatePhone(CurrentEmployee.TableNumber, phone_formatted);
            client.Close();
        }

        private void ExecuteDeclineCommand(object obj)
        {
            DatabaseServiceClient client = new DatabaseServiceClient();
            client.DeclineClaim(CurrentClaim);
            client.Close();
        }

        private void ExecuteCloseCommand(object obj)
        {
            DatabaseServiceClient client = new DatabaseServiceClient();
            client.CloseClaim(CurrentClaim);
            client.Close();
        }

        private void ExecuteConfirmCommand(object obj)
        {
            DatabaseServiceClient client = new DatabaseServiceClient();
            client.AcceptClaim(CurrentClaim);
            client.Close();
        }

        private void ExecuteFinishCommand(object obj)
        {
            DatabaseServiceClient client = new DatabaseServiceClient();
            client.FinishClaim(CurrentClaim);
            client.Close();
        }

        private void ExecuteAcceptCommand(object obj)
        {
            DatabaseServiceClient client = new DatabaseServiceClient();
            MessageBox.Show("Вы действительно хотите принять заявку?", 
                "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            client.ExecuteClaim(CurrentClaim, CurrentEmployee.TableNumber);
            client.Close();
        }

        private void ExecuteSetExecutorCommand(object obj)
        {
            int id = (int)obj;
            DatabaseServiceClient client = new DatabaseServiceClient();
            var depts = client.GetDepartments();
            client.SendExecuteClaim(CurrentClaim, depts.ElementAt(id));
            client.Close();
        }

        private void ExecuteApproveCommand(object obj)
        {
            DatabaseServiceClient client = new DatabaseServiceClient();
            client.ApproveClaim(CurrentClaim, CurrentEmployee.TableNumber);
            client.Close();
        }

        private bool CanExecuteApproveCommand(object arg)
        {
            bool result;
            try { result = CurrentEmployee.Department == 9 && CurrentClaim.Status.Code == 1; }
            catch (Exception) { return false; }            
            return result;            
        }

        private bool CanExecuteSetExecutorCommand(object arg)
        {
            bool result;
            try { result = CurrentEmployee.Department == 9 && CurrentClaim.Status.Code == 2; }
            catch (Exception) { return false; }
            return result;
        }

        private bool CanExecuteAcceptCommand(object arg)
        {
            bool result;
            try { result = CurrentEmployee.Department == CurrentClaim.Department && CurrentClaim.Status.Code == 3; }
            catch (Exception) { return false; }
            return result;            
        }

        private bool CanExecuteFinishCommand(object arg)
        {
            bool result;
            try { result = CurrentEmployee.TableNumber == CurrentClaim.Executor && CurrentClaim.Status.Code == 4; }
            catch (Exception) { return false; }
            return result;
        }

        private bool CanExecuteConfirmCommand(object arg)
        {
            bool result;
            try { result = CurrentEmployee.TableNumber == CurrentClaim.Initiator && CurrentClaim.Status.Code == 5; }
            catch (Exception) { return false; }
            return result;
        }

        private bool CanExecuteCloseCommand(object arg)
        {
            bool result;
            try { result = CurrentEmployee.Department == 9 && CurrentClaim.Status.Code == 6; }
            catch (Exception) { return false; }
            return result;
        }
    }
}
