using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.model
{
    public class UserDataResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }

    public class AuthenticationModel : INotifyPropertyChanged
    {
        #region private members
        private Int32 id;

        private string name;
        private string email;
        private string password;
        private string _chatChannel;
        private string token;

        private bool isLoggedIn;
        #endregion

        #region Public properties
        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set
            {
                isLoggedIn = value;
            }
        }

        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        public string ChatChannel
        {
            get { return _chatChannel; }
            set
            {
                _chatChannel = value;
                NotifyPropertyChanged("ChatChannel");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }


        public string Email
        {
            get { return email; }
            set {
                email = value;
                NotifyPropertyChanged("Email");
            }
        }

        public string Password
        {
            get { return password; }
            set {
                password = value;
                NotifyPropertyChanged("Password");
            }
        }
        #endregion

        #region notifypropertychanged members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }

    public record struct SignInDto(
        string email,
        string password
    );

    public record struct SignOutDto(
        string token
    );
}
