using ClientApplication.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace ClientApplication.Views
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                if (DataContext is IClosable vm)
                    vm.Close += () => this.Close();
            };
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
