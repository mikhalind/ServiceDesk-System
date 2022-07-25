using ClientApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ClientApplication.Helpers
{
    public static class WindowsContainer
    {
        private static Dictionary<Type, Type> Mappings = new Dictionary<Type, Type>();

        public static void RegisterDialog<TViewModel, TView>() => Mappings.Add(typeof(TViewModel), typeof(TView));

        public static void Display<TViewModel>(Action<object> closedCallback, bool modal = true)
        {
            var vType = Mappings[typeof(TViewModel)];
            var vResult = CreateInstance(vType) as Window;

            if (vResult != null)
            {
                EventHandler closedEventHandler = null;

                closedEventHandler = (s, e) =>
                {
                    closedCallback(vResult.DataContext);
                    vResult.Closed -= closedEventHandler;
                };

                vResult.Closed += closedEventHandler;

                if (modal) vResult.ShowDialog(); 
                else vResult.Show();
            }
        }

        private static object CreateInstance(Type implementationType) => Activator.CreateInstance(implementationType);
    }
}
