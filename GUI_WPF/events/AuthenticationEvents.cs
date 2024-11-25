using GUI.model;
using GUI_WPF.service;
using GUI_WPF.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI_WPF.events
{
    public class AuthenticationEventArg : System.EventArgs
    {
        #region Private members
        private AuthenticationModel _model;
        #endregion

        #region Public members
        public AuthenticationModel Model { get { return _model; } }
        #endregion

        public AuthenticationEventArg(AuthenticationModel _model)
        {
            this._model = _model;
        }
    }

    public class AuthenticationEvents
    {
        #region References
        /* Models */
        private AuthenticationModel _model;
        private AuthenticationService _service;
        #endregion

        #region Registered event handlers
        public event EventHandler<AuthenticationEventArg>? SubmitLoginInfoEvent;
        public event EventHandler<AuthenticationEventArg>? SubmitLogoutInfoEvent;
        #endregion

        #region Commands
        public DelegateCommand SubmitLoginInfoCommand { get; set; }
        public DelegateCommand SubmitLogoutInfoCommand { get; set; }
        #endregion

        public AuthenticationEvents(AuthenticationModel am)
        {
            this._model=am;

            this._service = new();

            /* Register commands */
            SubmitLoginInfoCommand = new DelegateCommand(param => OnSubmitLoginInfo());
            SubmitLogoutInfoCommand = new DelegateCommand(param => OnSubmitLogoutInfo());

            /* Register event handlers */
            SubmitLoginInfoEvent += new EventHandler<AuthenticationEventArg>(SubmitLogin);
            SubmitLogoutInfoEvent += new EventHandler<AuthenticationEventArg>(SubmitLogout);
        }

        #region Events that run after executing a command
        private void OnSubmitLoginInfo()
        {
            SubmitLoginInfoEvent?.Invoke(this, new AuthenticationEventArg(_model));
        }

        private void OnSubmitLogoutInfo()
        {
            SubmitLogoutInfoEvent?.Invoke(this, new AuthenticationEventArg(_model));
        }
        #endregion




        #region Methods that will run after invoke an event
        /* Event of sign in action */
        public async void SubmitLogin(object? sender, AuthenticationEventArg e)
        {
            if (string.IsNullOrEmpty(e.Model.Email))
            {
                MessageBox.Show("Whoops! The email field cannot be empty!");
                return;
            }

            if (string.IsNullOrEmpty(e.Model.Password))
            {
                MessageBox.Show("Whoops! The password field cannot be empty!");
                return;
            }

            if (string.IsNullOrEmpty(e.Model.ChatChannel))
            {
                MessageBox.Show("Whoops! The chat channel field cannot be empty!");
                return;
            }

            try
            {
                string[] splitted = e.Model.ChatChannel.Split(":");

                if (splitted is null || splitted.Length == 0)
                {
                    MessageBox.Show("Invalid chat channel format preferred(127.0.0.1:2546) (host:port)!");
                    return;
                }

                string host = splitted[0];
                Int32 port = Int32.Parse(splitted[1]);

                App.CHAT_SERVER_HOST = host;
                App.CHAT_SERVER_PORT = port;
            } catch (Exception ex)
            {
                MessageBox.Show("Invalid chat channel format preferred(127.0.0.1:2546) (host:port)!");
                return;
            }

            /* Create sign in action & store values in a registry */
            await _service.SignIn(e.Model);
        }

        /* Event of sign out action */
        public async void SubmitLogout(object? sender, AuthenticationEventArg e)
        {
            /* Create sign out action & remove values from the registry */
            await _service.SignOut(e.Model);
        }
        #endregion
    }
}
