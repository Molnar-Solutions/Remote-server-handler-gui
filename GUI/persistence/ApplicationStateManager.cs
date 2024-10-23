using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.persistence
{
    public class ApplicationStateManager
    {
        private static ApplicationStateManager _instance;
        private ApplicationState _state;

        private ApplicationStateManager()
        {
            _state = new ApplicationState();
        }

        public static ApplicationStateManager getInstance()
        {
            if (_instance == null)
            {
                _instance = new ApplicationStateManager();
            }

            return _instance;
        }

        public ApplicationState getState()
        {
            return _state;
        }
    }
}
