using Encryptions;
using GUI.model;
using GUI_WPF.events;
using GUI_WPF.viewModel;
using System.Configuration;
using System.Data;
using System.Net.Sockets;
using System.Text;
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

        #region Constants
        public static string ENCRYPTION_KEY = "asdgasd3lklj23ljfh2ou3hro28f48hh4fhl8chjvhjw4848483ldj//!!++";
        public static string CHAT_SERVER_HOST = "127.0.0.1";
        public static Int32 CHAT_SERVER_PORT = 2546;
        public static Int32 ACTIVE_USER_ID = -1;
        #endregion

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
