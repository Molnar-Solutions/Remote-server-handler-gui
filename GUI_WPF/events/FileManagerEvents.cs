using Data.model;
using GUI.model;
using GUI_WPF.service;
using GUI_WPF.viewModel;
using MD_Store.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI_WPF.events
{
    public class FileManagerEventArg : System.EventArgs
    {
        #region Private members
        private FileManagerModel _model;
        private FileTableDataModel _tableData;
        #endregion

        #region Public members
        public FileManagerModel Model { get { return _model; } }
        public FileTableDataModel FileTableDataModel { get { return _tableData; } }
        #endregion

        public FileManagerEventArg(FileManagerModel _model, FileTableDataModel tableData)
        {
            this._model = _model;
            _tableData = tableData;
        }
    }

    public class FileManagerEvents
    {
        #region Useful variables
        private static bool isClicked = true;
        #endregion

        #region References
        private FileManagerModel _model;
        private FileTableDataModel _fileTableDataModel;
        private FileManagerService _service;
        private RegistryConfig _registryConfig;
        #endregion

        #region Registered event handlers
        public event EventHandler<FileManagerEventArg> FileManagerMenuEvent;
        public event EventHandler<FileManagerEventArg>? UploadFileEvent;
        public event EventHandler<FileManagerEventArg>? RemoveFileEvent;
        public event EventHandler<FileManagerEventArg>? DownloadFileEvent;
        #endregion

        #region Commands
        public DelegateCommand FileManagerMenuCommand { get; set; }
        public DelegateCommand UploadFileCommand { get; set; }
        public DelegateCommand RemoveFileCommand { get; set; }
        public DelegateCommand DownloadFileCommand { get; set; }
        #endregion

        public FileManagerEvents(FileManagerModel fm, FileTableDataModel ftd, Grid fileManagerGrid)
        {
            this._model=fm;
            this._fileTableDataModel=ftd;
            this._registryConfig = RegistryConfig.Init();

            /* Create service instance */
            _service = new();

            /* Register commands */
            UploadFileCommand = new DelegateCommand(param => OnUploadFile());
            RemoveFileCommand = new DelegateCommand(param => OnRemoveFile());
            DownloadFileCommand = new DelegateCommand(param => OnDownloadFile());
            FileManagerMenuCommand = new DelegateCommand(param => OnOpenFileManager(fileManagerGrid));

            /* Register event handlers */
            UploadFileEvent += new EventHandler<FileManagerEventArg>(UploadFile);
            RemoveFileEvent += new EventHandler<FileManagerEventArg>(RemoveFile);
            DownloadFileEvent += new EventHandler<FileManagerEventArg>(DownloadFile);
            FileManagerMenuEvent += new EventHandler<FileManagerEventArg>(OpenFileManager);
        }

        #region Events that run after executing a command
        private void OnUploadFile()
        {
            /* Check logged in status */
            string? registryValue = _registryConfig.GetConfigValue($"{App.ACTIVE_USER_ID}", "isLoggedIn");
            bool isLoggedIn = false;

            if (!string.IsNullOrEmpty(registryValue))
            {
                isLoggedIn = Boolean.Parse(registryValue);
            }

            if (isLoggedIn == false)
            {
                MessageBox.Show("Whoops! You are not logged in!");
                return;
            }

            /* Raise event */
            UploadFileEvent?.Invoke(this, new FileManagerEventArg(_model, _fileTableDataModel));
        }

        private void OnRemoveFile()
        {
            /* Check logged in status */
            string? registryValue = _registryConfig.GetConfigValue($"{App.ACTIVE_USER_ID}", "isLoggedIn");
            bool isLoggedIn = false;

            if (!string.IsNullOrEmpty(registryValue))
            {
                isLoggedIn = Boolean.Parse(registryValue);
            }

            if (isLoggedIn == false)
            {
                MessageBox.Show("Whoops! You are not logged in!");
                return;
            }

            /* Raise event */
            RemoveFileEvent?.Invoke(this, new FileManagerEventArg(_model, _fileTableDataModel));
        }

        private void OnDownloadFile()
        {
            /* Check logged in status */
            string? registryValue = _registryConfig.GetConfigValue($"{App.ACTIVE_USER_ID}", "isLoggedIn");
            bool isLoggedIn = false;

            if (!string.IsNullOrEmpty(registryValue))
            {
                isLoggedIn = Boolean.Parse(registryValue);
            }

            if (isLoggedIn == false)
            {
                MessageBox.Show("Whoops! You are not logged in!");
                return;
            }

            /* Raise event */
            DownloadFileEvent?.Invoke(this, new FileManagerEventArg(_model, _fileTableDataModel));
        }

        private void OnOpenFileManager(Grid fileManagerGrid)
        {
            /* Check logged in status */
            string? registryValue = _registryConfig.GetConfigValue($"{App.ACTIVE_USER_ID}", "isLoggedIn");
            bool isLoggedIn = false;

            if (!string.IsNullOrEmpty(registryValue))
            {
                isLoggedIn = Boolean.Parse(registryValue);
            }

            if (isLoggedIn == false)
            {
                MessageBox.Show("Whoops! You are not logged in!");
                return;
            }

            /* Show grid */
            if (isClicked)
            {
                fileManagerGrid.Visibility = Visibility.Visible;
                isClicked = !isClicked;
            }
            else
            {
                fileManagerGrid.Visibility = Visibility.Hidden;
                isClicked = !isClicked;
            }


            /* Raise event */
            FileManagerMenuEvent.Invoke(this, new FileManagerEventArg(_model, _fileTableDataModel));
        }
        #endregion

        #region Methods that will run after invoke an event
        /* Event of upload file action */
        public async void UploadFile(object? sender, FileManagerEventArg e)
        {
            if (e == null)
            {
                MessageBox.Show("Whoops! The event is null!");
                return;
            }

            if (e.Model == null)
            {
                MessageBox.Show("Whoops! The event data[table contents] is null!");
                return;
            }

            if (e.FileTableDataModel == null)
            {
                MessageBox.Show("Please select a line!");
                return;
            }

            await _service.addFile(e.Model);
        }

        /* Event of remove file action */
        public async void RemoveFile(object? sender, FileManagerEventArg e)
        {
            if (e == null)
            {
                MessageBox.Show("Whoops! The event is null!");
                return;
            }

            if (e.Model == null)
            {
                MessageBox.Show("Whoops! The event data[table contents] is null!");
                return;
            }

            if (e.FileTableDataModel == null)
            {
                MessageBox.Show("Please select a line!");
                return;
            }

            await _service.removeFile(e.Model);
        }

        /* Event of download action */
        public async void DownloadFile(object? sender, FileManagerEventArg e)
        {
            if (e == null)
            {
                MessageBox.Show("Whoops! The event is null!");
                return;
            }

            if (e.Model == null)
            {
                MessageBox.Show("Whoops! The event data[table contents] is null!");
                return;
            }

            if (e.FileTableDataModel == null)
            {
                MessageBox.Show("Please select a line!");
                return;
            }

            await _service.downloadFile(e.Model);
        }

        /* Event of open file manager action */
        public async void OpenFileManager(object? sender, FileManagerEventArg e)
        {
            await _service._loadFileContents(e.Model);
        }
        #endregion
    }
}
