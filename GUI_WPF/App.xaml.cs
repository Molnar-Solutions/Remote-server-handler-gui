using GUI.model;
using GUI_WPF.events;
using GUI_WPF.viewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GUI_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        private MainWindow _mainWindow;

        /* View model and main context(s) */
        private MainViewModel _mainViewModel;

        public App()
        {
            Startup += new StartupEventHandler((object sender, StartupEventArgs e) =>
            {
                /* Initialize window(s) */
                _mainWindow = new();

                /* Initialize view model(s) */
                _mainViewModel = new(_mainWindow);

                /* Assign data contexts */
                _mainWindow.DataContext = _mainViewModel;

                _mainWindow.Show();
            });
        }
    }
}
