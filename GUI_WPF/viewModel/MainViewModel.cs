using Data.model;
using GUI.model;
using GUI_WPF.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI_WPF.viewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Private members
        /* Main window */
        private MainWindow _mainWindow;

        /* Models */
        private AuthenticationModel _model;

        private FileManagerModel _fileManagerModel;
        private FileTableDataModel _fileTableDataModel;
        #endregion

        #region Main properties
        public AuthenticationModel AuthModel
        {
            get { return _model; }
            set { _model = value; }
        }

        public FileManagerModel FileManagerModel
        {
            get { return _fileManagerModel; }
            set { _fileManagerModel = value; }
        }

        public FileTableDataModel FileTableDataModel
        {
            get { return _fileTableDataModel; }
            set { _fileTableDataModel = value; }
        }

        #endregion

        #region Event classes
        public AuthenticationEvents AuthenticationEvents {  get; private set; }
        public FileManagerEvents FileManagerEvents { get; private set; }
        #endregion

        #region Constructors
        public MainViewModel(MainWindow window)
        { 
            this._model = new();
            this._fileManagerModel = new FileManagerModel();
            this._fileTableDataModel = new FileTableDataModel();

            this._mainWindow = window;

            Grid fileManagerGrid = (Grid)_mainWindow.FindName("fileManagerGrid") ?? null;
            Grid chatManagerGrid = (Grid)_mainWindow.FindName("chatGrid") ?? null;

            if (fileManagerGrid != null)
            {
                chatManagerGrid.Visibility = Visibility.Hidden;
                fileManagerGrid.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Whoops! I cannot find the filemanagergrid!");
                return;
            }

            this.AuthenticationEvents = new(_model);
            this.FileManagerEvents = new(_fileManagerModel, _fileTableDataModel, fileManagerGrid);
        }
        #endregion
    }
}
