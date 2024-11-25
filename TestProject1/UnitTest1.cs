using GUI.model;
using GUI.utils;
using MD_Networking;
using MD_Networking.Exceptions;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Encryptions;
using MD_Store.Lib;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Collections.Concurrent;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task AuthenticationTest()
        {
            /* Authentication */
            using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
            {
                APIResponse userResponse = await client.PostAsync<SignInDto>("user/sign-in", new()
                {
                    email = "test@gmail.com",
                    password = "asd123"
                });

                Assert.AreEqual(true, (userResponse.StatusCode >= 200 && userResponse.StatusCode < 300));
            }
        }

        [TestMethod]
        public async Task UserDataConvertTest()
        {
            /* Authentication */
            using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
            {
                /* Get User Data */
                APIResponse userResponse = await client.PostAsync<SignInDto>("user/sign-in", new()
                {
                    email = "test@gmail.com",
                    password = "asd123"
                });

                /* Read data from the inside property & convert to userdataresponse */
                StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(userResponse.Data), Encoding.UTF8, "application/json");
                UserDataResponse? userData = System.Text.Json.JsonSerializer.Deserialize<UserDataResponse>(await jsonContent.ReadAsStringAsync());

                Assert.AreEqual(true, (userResponse.StatusCode >= 200 && userResponse.StatusCode < 300) && !(userData is null));
            }
        }

        [TestMethod]
        public async Task GetFilesFromServer()
        {
            /* Authentication */
            using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
            {
                /* Get user data */
                APIResponse userResponse = await client.PostAsync<SignInDto>("user/sign-in", new()
                {
                    email = "test@gmail.com",
                    password = "asd123"
                });

                /* Read data from the inside property & convert to userdataresponse */
                StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(userResponse.Data), Encoding.UTF8, "application/json");
                UserDataResponse? userData = System.Text.Json.JsonSerializer.Deserialize<UserDataResponse>(await jsonContent.ReadAsStringAsync());

                /* Get file contents */
                APIResponse response = await client.PostAsync<GetFilesDto>("connector/list-files", new()
                {
                    email = userData.email
                });

                /* Convert the embedded data object to json serialized string */
                StringContent filesJsonContent = new(System.Text.Json.JsonSerializer.Serialize(response.Data), Encoding.UTF8,
                    "application/json");

                IEnumerable<FileTableDataModel>? files = System.Text.Json.JsonSerializer
                    .Deserialize<IEnumerable<FileTableDataModel>>(await filesJsonContent.ReadAsStringAsync());

                Assert.IsTrue(files.Any() || !(response.Data is null));
            }
        }

        [TestMethod]
        public async Task GetSystemInfos()
        {
            /* Authentication */
            using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
            {
                /* Get user data */
                APIResponse userResponse = await client.PostAsync<SignInDto>("user/sign-in", new()
                {
                    email = "test@gmail.com",
                    password = "asd123"
                });

                /* Read data from the inside property & convert to userdataresponse */
                StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(userResponse.Data), Encoding.UTF8, "application/json");
                UserDataResponse? userData = System.Text.Json.JsonSerializer.Deserialize<UserDataResponse>(await jsonContent.ReadAsStringAsync());

                /* Get health contents */
                APIResponse response = await client.PostAsync<SystemHealthRequest>("connector/system-health", new()
                {
                    email = userData.email
                });

                /* Convert response data to SysteamHealthResponse object */
                SysteamHealthResponse responseData = JsonConvert.DeserializeObject<SysteamHealthResponse>(response.Data.ToString());


                Assert.IsTrue(!(responseData.Equals(null)) && !(response.Data is null));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NetworkCommunicationException))]
        public async Task CatchInvalidRequest()
        {
            /* Authentication */
            using (NetworkClient client = new NetworkClient("http://localhost:9998/blabla"))
            {
                /* Get user data */
                SignInDto userResponse = await client.PostOrThrowAsync<SignInDto>("user/sign-in", new()
                {
                    email = "test@gmail.com",
                    password = "asd123"
                });
            }
        }

        [TestMethod]
        public async Task CheckConcurrency()
        {
            ConcurrentQueue<string> messages = new ConcurrentQueue<string>();

            const int threadCount = 100;
            var barrier = new Barrier(threadCount + 1);

            var tasks = new List<Task>();

            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    barrier.SignalAndWait();

                    messages.Enqueue("Heeellooo");
                }));
            }

            barrier.SignalAndWait();
            await Task.WhenAll(tasks.ToArray());
            Assert.AreEqual(threadCount, messages.Count);
        }

        [TestMethod]
        public async Task CheckServerConnection()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 2546);
            listener.Start();

            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, 2546);

            Assert.AreEqual(true, client.Connected);

            listener.Stop();
        }

        [TestMethod]
        public async Task EncryptionDecryptionCheck()
        {
            string ENCRYPTION_KEY = "asdgasd3lklj23ljfh2ou3hro28f48hh4fhl8chjvhjw4848483ldj//!!++";

            byte[] sendingData = Encoding.UTF8.GetBytes("Hello world!");
            sendingData = Encryption.EncryptAesCBC(sendingData, ENCRYPTION_KEY);

            byte[] decrypted = Encryption.DecryptAesCBC(_extractData(sendingData), ENCRYPTION_KEY);
            string messageFromServer = Encoding.UTF8.GetString(decrypted);

            Assert.AreEqual("Hello world!", messageFromServer);
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