using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.model;
using GUI.utils;
using MD_Networking;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUI.testing
{
    [TestClass]
    public class ProjectTester
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

                Assert.AreEqual(false, (userResponse.StatusCode >= 200 && userResponse.StatusCode < 300));
            }
        }
    }
}
