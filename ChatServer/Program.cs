using Encryptions;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatServer
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string MessageContent { get; set; }
        public DateTime SentTime { get; set; } = DateTime.Now;
    }

    public class ChatServer
    {
        public ConcurrentQueue<ChatMessage> Messages { get; } = new ConcurrentQueue<ChatMessage>();
    }

    public class Program
    {
        private static Int32 PORT = 2546;
        private static volatile bool IS_RUNNING = true;

        static readonly List<TcpClient> clients = new();

        private static string _encryptionKey = "asdgasd3lklj23ljfh2ou3hro28f48hh4fhl8chjvhjw4848483ldj//!!++";

        static async Task Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            Console.WriteLine("* Server started on port " + PORT);

            List<TcpClient> connectedClients = new List<TcpClient>(); // Use a list for connected clients

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                connectedClients.Add(client);

                Console.WriteLine("* Client connected: " + client.Client.RemoteEndPoint);

                Task.Run(() => HandleClient(client, connectedClients)); // Pass the client list
            }
        }

        static async Task HandleClient(TcpClient client, List<TcpClient> connectedClients)
        {
            NetworkStream stream = client.GetStream();

            while (client.Connected)
            {
                try
                {
                    byte[] buffer = new byte[2048];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        Console.WriteLine("Client disconnected: " + client.Client.RemoteEndPoint);
                        connectedClients.Remove(client);
                        client.Close();
                        break;
                    }

                    byte[] decrypted = Encryption.DecryptAesCBC(_extractData(buffer), _encryptionKey);
                    string message = Encoding.UTF8.GetString(decrypted);

                    Console.WriteLine("Received message from " + client.Client.RemoteEndPoint + ": " + message);

                    // Broadcast message to all connected clients
                    foreach (TcpClient c in connectedClients)
                    {
                        // If I would like to skip the client then if (c != client) { ... }
                        try
                        {
                            /* Encrypt the msg */
                            byte[] sendingData = Encoding.UTF8.GetBytes(message);
                            sendingData = Encryption.EncryptAesCBC(sendingData, _encryptionKey);

                            /* Send it */
                            await c.GetStream().WriteAsync(sendingData);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending message to client {c.Client.RemoteEndPoint}: {ex.Message}");
                            connectedClients.Remove(c);
                            c.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error handling client: " + ex.Message);
                    connectedClients.Remove(client);
                    client.Close();
                    break;
                }
            }
        }

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
    }
}
