using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.model
{
    public class ChatModel
    {
        private string _name;
        private string _currentMessage;
        private ObservableCollection<string> _messages = new();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string CurrentMessage
        {
            get { return _currentMessage; }
            set { _currentMessage = value; }
        }

        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        public void AddMessage(string messsage)
        {
            Messages.Add(messsage);
        }
    }
}
