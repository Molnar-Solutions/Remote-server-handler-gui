using GUI.model;
using GUI.utils;
using MD_Networking;
using MD_Networking.Exceptions;
using Newtonsoft.Json;
using System.Text;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task AuthenticationTest()
        {
            /* Authentication */
            using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
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
            using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
            {
                /* Get User Data */
                APIResponse userResponse = await client.PostAsync<SignInDto>("user/sign-in", new()
                {
                    email = "test@gmail.com",
                    password = "asd123"
                });

                /* Read data from the inside property & convert to userdataresponse */
                StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(userResponse.Data), Encoding.UTF8, "application/json");
                UserDataResponse userData = System.Text.Json.JsonSerializer.Deserialize<UserDataResponse>(await jsonContent.ReadAsStringAsync());

                Assert.AreEqual(true, (userResponse.StatusCode >= 200 && userResponse.StatusCode < 300) && !(userData is null));
            }
        }

        [TestMethod]
        public async Task GetFilesFromServer()
        {
            /* Authentication */
            using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
            {
                /* Get user data */
                APIResponse userResponse = await client.PostAsync<SignInDto>("user/sign-in", new()
                {
                    email = "test@gmail.com",
                    password = "asd123"
                });

                /* Read data from the inside property & convert to userdataresponse */
                StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(userResponse.Data), Encoding.UTF8, "application/json");
                UserDataResponse userData = System.Text.Json.JsonSerializer.Deserialize<UserDataResponse>(await jsonContent.ReadAsStringAsync());

                /* Get file contents */
                APIResponse response = await client.PostAsync<GetFilesDto>("connector/list-files", new()
                {
                    email = userData.email
                });

                /* Convert the embedded data object to json serialized string */
                StringContent filesJsonContent = new(System.Text.Json.JsonSerializer.Serialize(response.Data), Encoding.UTF8,
                    "application/json");

                IEnumerable<FileTableDataModel> files = System.Text.Json.JsonSerializer
                    .Deserialize<IEnumerable<FileTableDataModel>>(await filesJsonContent.ReadAsStringAsync());

                Assert.IsTrue(files.Any() || !(response.Data is null));
            }
        }

        [TestMethod]
        public async Task GetSystemInfos()
        {
            /* Authentication */
            using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
            {
                /* Get user data */
                APIResponse userResponse = await client.PostAsync<SignInDto>("user/sign-in", new()
                {
                    email = "test@gmail.com",
                    password = "asd123"
                });

                /* Read data from the inside property & convert to userdataresponse */
                StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(userResponse.Data), Encoding.UTF8, "application/json");
                UserDataResponse userData = System.Text.Json.JsonSerializer.Deserialize<UserDataResponse>(await jsonContent.ReadAsStringAsync());

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
            using (NetworkClient client = new NetworkClient("http://localhost:3000/blabla"))
            {
                /* Get user data */
                SignInDto userResponse = await client.PostOrThrowAsync<SignInDto>("user/sign-in", new()
                {
                    email = "test@gmail.com",
                    password = "asd123"
                });
            }
        }
    }
}