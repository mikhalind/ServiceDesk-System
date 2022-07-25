using ClientApplication.DBService;
using ClientApplication.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ClientApplication.ViewModels
{
    public struct FormData
    {
        public string theme;
        public string description;
        public int typeCode;
        public int categoryCode;
        public int currentEmployee;
    }

    public class InputValuesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            FormData formData = new FormData()
            {
                theme = values[0].ToString(),
                description = values[1].ToString(),
                typeCode = Int32.Parse(values[2].ToString()),
                categoryCode = Int32.Parse(values[3].ToString()),
                currentEmployee = Int32.Parse(values[4].ToString())
            };

            return (object)formData;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class AddClaimWindowViewModel : BaseViewModel, IClosable
    {
        public ClaimEntity currentClaim;
        private Action close;        
        private List<CategoryEntity> categories;
        private List<TypeEntity> types;

        public CreateClaimCommand CreateClaimCmd { get; set; }

        public List<CategoryEntity> Categories
        {
            get { return categories; }
            set { categories = value; } 
        }

        public List<TypeEntity> Types
        {
            get { return types; }
            set { types = value; }
        }

        public Action Close
        {
            get { return close; }
            set { SetProperty(ref close, value); }
        }

        public AddClaimWindowViewModel()
        {
            DBService.DatabaseServiceClient client = new DBService.DatabaseServiceClient();
            Categories = client.GetCategories().ToList();
            Types = client.GetTypes().ToList();
            CreateClaimCmd = new CreateClaimCommand((obj) => CreateClaim(obj));
        }

        public void CreateClaim(object param)
        {
            var value = (FormData)param;
            string theme = value.theme;
            string description = value.description;
            int category = value.categoryCode;
            int type = value.typeCode;
            int curEmp = value.currentEmployee;
            
            DatabaseServiceClient client = new DatabaseServiceClient();
            currentClaim = new ClaimEntity()
            {
                CreatedDate = DateTime.Now,
                Theme = theme,
                Description = description,
                Initiator = curEmp,
                Status = client.GetStatuses().FirstOrDefault(i => i.Code == 1),
                Category = client.GetCategories().FirstOrDefault(i => i.Code == category),
                ClaimType = client.GetTypes().FirstOrDefault(i => i.ID == type)
            };
            
            MessageBox.Show(description, theme);
        }
    }
}
