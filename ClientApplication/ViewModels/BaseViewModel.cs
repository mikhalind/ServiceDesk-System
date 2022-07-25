using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.ViewModels
{
    public abstract class BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storage, value))
                return false;
            else
            {
                storage = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    }

    public interface IClosable
    {
        Action Close { get; set; }
    }
}
