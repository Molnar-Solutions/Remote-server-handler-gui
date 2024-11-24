using GUI.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.model
{
    public class FileManagerModel
    {
        #region private members
        private FileTableDataModel _selectedElement;
        private ObservableCollection<FileTableDataModel> _tableContents;
        #endregion

        public FileManagerModel()
        {
            _selectedElement = new FileTableDataModel();
            _tableContents = new ObservableCollection<FileTableDataModel>();
        }

        #region Public properties 
        public ObservableCollection<FileTableDataModel> TableContents {
            get {  return _tableContents; }
            set { _tableContents = value; }
        }

        public FileTableDataModel SelectedElement
        {
            get { return _selectedElement; }
            set { _selectedElement = value; }
        }

        #endregion
    }
}
