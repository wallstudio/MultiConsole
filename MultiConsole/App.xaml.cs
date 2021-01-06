using MultiConsole.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MultiConsole
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        WindowViewModel mainViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            mainViewModel = new WindowViewModel();
        }
    }
}
