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
        private List<FileTableDataModel> _tableContents;
        #endregion

        #region Public properties
        public List<FileTableDataModel> TableContents {
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
