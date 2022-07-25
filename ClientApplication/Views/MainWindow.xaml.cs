using ClientApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApplication.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                if (DataContext is IClosable vm)
                    vm.Close += () => this.Close();
            };
        }
    }
}
