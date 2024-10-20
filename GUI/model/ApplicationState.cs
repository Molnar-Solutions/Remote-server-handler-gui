using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.model
{

    public class ApplicationState
    {
        public bool isLoggedIn { get; set; }
        public UserDataResponse? userData { get; set; }
        public IEnumerable<FileTableDataModel>? files { get; set; }

        public ApplicationState()
        {
            isLoggedIn = false;
        }
    }
}
