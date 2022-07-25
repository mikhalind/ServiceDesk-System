using ClientApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientApplication.Views
{
    public partial class MessageWindow : Window
    {
        public MessageWindow()
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
