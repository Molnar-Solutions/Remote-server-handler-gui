using Data.model;
using GUI.model;
using GUI_WPF.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            this.AuthenticationEvents = new(_model);
            this.FileManagerEvents = new(_fileManagerModel, _fileTableDataModel);

            /* Add data contexts */
            /* Sign In / Sign Out */
            _mainWindow.emailTextBox.DataContext = _model;
            _mainWindow.passwordTextBox.DataContext = _model;
            _mainWindow.apiUrlTextBox.DataContext = _model;

            _mainWindow.filesDataGrid.ItemsSource = _fileManagerModel.TableContents;
        }
        #endregion
    }
}
