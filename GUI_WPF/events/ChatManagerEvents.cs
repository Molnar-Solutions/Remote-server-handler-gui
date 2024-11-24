using Data.model;
using Encryptions;
using GUI_WPF.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GUI_WPF.events
{
    public class ChatManagerEventArg : System.EventArgs
    {
        #region Private members
        private ChatModel _chatModel;
        #endregion

        #region Public members
        public ChatModel ChatModel { get { return _chatModel; } }
        #endregion

        public ChatManagerEventArg(ChatModel ChatModel)
        {
            this._chatModel = ChatModel;
        }
    }

    public class ChatManagerEvents
    {
        #region Useful variables
        private static bool isClicked = true;
        #endregion

        #region References
        private ChatModel _chatModel;
        private Grid _chatManagerGrid;
        private TcpClient _tcpClient;
        private Dispatcher dispatcher;
        #endregion

        #region Registered event handlers
        public event EventHandler<ChatManagerEventArg> SubmitClientMessageEvent;
        #endregion

        #region Commands
        public DelegateCommand OpenChatMenu { get; set; }
        public DelegateCommand SubmitClientMessage { get; set; }
        #endregion

        public ChatManagerEvents(ChatModel cm, System.Windows.Controls.Grid chatManagerGrid)
        {
            this._chatModel= cm;
            this._chatManagerGrid = chatManagerGrid;

            /* Register commands */
            SubmitClientMessage = new DelegateCommand(param => OnSubmitClientMessage());
            OpenChatMenu = new DelegateCommand(param => OnOpenChatMenu());

            /* Register event handlers */
            SubmitClientMessageEvent += new EventHandler<ChatManagerEventArg>(SubmitMessage);
        }

        #region Events that run after executing the command
        private void OnSubmitClientMessage()
        {
            SubmitClientMessageEvent?.Invoke(this, new(_chatModel));
        }

        private void OnOpenChatMenu()
        {
            /* Show grid */
            if (isClicked)
            {
                _chatManagerGrid.Visibility = Visibility.Visible;
                isClicked = !isClicked;
            }
            else
            {
                _chatManagerGrid.Visibility = Visibility.Hidden;
                isClicked = !isClicked;
            }
        }
        #endregion

        #region Methods that will run after invoke an event
        public async void SubmitMessage(object? sender, ChatManagerEventArg args)
        {
            if (string.IsNullOrEmpty(args.ChatModel.CurrentMessage))
            {
                return;
            }

            try
            {
                if (_tcpClient == null || !_tcpClient.Connected)
                {
                    _tcpClient = new TcpClient();
                    await _tcpClient.ConnectAsync(App.CHAT_SERVER_HOST, App.CHAT_SERVER_PORT);
                }

                using (var stream = _tcpClient.GetStream())
                {
                    /* Encrypt the msg */
                    byte[] sendingData = Encoding.UTF8.GetBytes(args.ChatModel.CurrentMessage);
                    sendingData = Encryption.EncryptAesCBC(sendingData, App.ENCRYPTION_KEY);

                    await stream.WriteAsync(sendingData);
                    await ReceiveMessages(_tcpClient, _chatModel);
                }
            }
            catch (Exception ex)
            {
                AddMessage($"Error: {ex.Message}", args.ChatModel);
            }
        }

        private async Task ReceiveMessages(TcpClient client, ChatModel model)
        {
            if (client == null || !client.Connected)
            {
                return;
            }

            NetworkStream stream = client.GetStream();

            while (client.Connected)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        MessageBox.Show("Disconnected from server");
                        client.Close();
                        break;
                    }

                    byte[] decrypted = Encryption.DecryptAesCBC(_extractData(buffer), App.ENCRYPTION_KEY);
                    string message = Encoding.UTF8.GetString(decrypted);
                    Console.WriteLine("Received message: " + message);

                    AddMessage(message, model);
                }
                catch (Exception)
                {
                    // Handle potential errors during message receive
                    AddMessage("Error receiving message", model);
                    break;
                }
            }
        }
        #endregion

        private static void AddMessage(string msg, ChatModel model)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                model.Messages.Add(msg);
            }));
        }

        #region Chat functions
        private static byte[] _extractData(byte[] input)
        {
            try
            {
                for (int i = 0; i < input.Length; i += 16)
                {
                    if (input[i] == 0)
                    {
                        return input[0..(i)];
                    }
                }
            }
            catch
            {
            }
            return input;
        }
        #endregion
    }
}
