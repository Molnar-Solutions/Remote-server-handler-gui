using GUI.model;
using GUI.utils;
using MD_Networking;
using MD_Store.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI_WPF.service
{
    public class AuthenticationService
    {
        private RegistryConfig _config;

        public AuthenticationService()
        {
            _config = RegistryConfig.Init();
        }

        public async Task SignIn(AuthenticationModel data)
        {
            try
            {
                /* Check logged in status */
                string? registryValue = _config.GetConfigValue("isLoggedIn");
                bool isLoggedIn = false;

                if (!string.IsNullOrEmpty(registryValue))
                {
                    isLoggedIn = Boolean.Parse(registryValue);
                }

                if (isLoggedIn == true)
                {
                    MessageBox.Show("Whoops! You are already logged in!");
                    return;
                }

                using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
                {
                    APIResponse userResponse = await client.PostAsync<SignInDto>("user/sign-in", new()
                    {
                        email = data.Email,
                        password = data.Password
                    });

                    System.Net.Http.StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(userResponse.Data), Encoding.UTF8, "application/json");
                    UserDataResponse userData = System.Text.Json.JsonSerializer.Deserialize<UserDataResponse>(await jsonContent.ReadAsStringAsync());


                    if (userData is null)
                    {
                        MessageBox.Show("Whoops! We could not load the user informations :(");
                        return;
                    }

                    /* Save response data to the registry */
                    _config.SetConfigValue("userId", $"{userData.id}");
                    _config.SetConfigValue("userEmail", $"{userData.email}");
                    _config.SetConfigValue("userName", $"{userData.name}");
                    _config.SetConfigValue("userToken", $"{userData.token}");
                    _config.SetConfigValue("isLoggedIn", "true");
                    /* Token is active for 8 hours, if the time gap is longer than I remove all the data that I set into the registry */
                    _config.SetConfigValue("loggedInTimeStamp", $"{DateTime.Now.ToString()}");

                    MessageBox.Show($"Hello, {userData.name}!");
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Whoops! Something went wrong during sign in! {ex.Message}");
            }
        }

        public async Task SignOut(AuthenticationModel data)
        {
            try
            {
                /* Check logged in status */
                string? registryValue = _config.GetConfigValue("isLoggedIn");
                bool isLoggedIn = false;

                if (!string.IsNullOrEmpty(registryValue))
                {
                    isLoggedIn = Boolean.Parse(registryValue);
                }

                if (isLoggedIn == false)
                {
                    MessageBox.Show("Whoops! You are not logged in!");
                    return;
                }

                string loggedInTimeStamp = _config.GetConfigValue("loggedInTimeStamp");

                if (string.IsNullOrEmpty(loggedInTimeStamp))
                {
                    /* The system do not save the logged in timestamp so remove everything */
                    _config.SetConfigValue("userId", "");
                    _config.SetConfigValue("userEmail", "");
                    _config.SetConfigValue("userName", "");
                    _config.SetConfigValue("userToken", "");
                    _config.SetConfigValue("loggedInTimeStamp", "");
                    _config.SetConfigValue("isLoggedIn", "false");
                    return;
                }

                var expireDate = DateTime.Parse(loggedInTimeStamp).AddHours(8);
                var now = DateTime.Now;
                int result = DateTime.Compare(now, expireDate);

                /* Token expiration */
                /* If expireDate is later than */
                if (now > expireDate)
                {
                    MessageBox.Show("Whoops! Your token is expired! You signed out by the system!");
                    _config.SetConfigValue("userId", "");
                    _config.SetConfigValue("userEmail", "");
                    _config.SetConfigValue("userName", "");
                    _config.SetConfigValue("userToken", "");
                    _config.SetConfigValue("loggedInTimeStamp", "");
                    _config.SetConfigValue("isLoggedIn", "false");
                    return;
                }

                string userToken = _config.GetConfigValue("userToken");

                if (string.IsNullOrEmpty(userToken))
                {
                    MessageBox.Show("Whoops! The token not found :( Try to sign in once again!");
                    return;
                }

                using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
                {
                    APIResponse userResponse = await client.PostAsync<SignOutDto>("user/sign-out", new()
                    {
                        token = userToken
                    });

                    if (!(userResponse.StatusCode >= 200 && userResponse.StatusCode < 300))
                    {
                        MessageBox.Show($"Whoops! Something went wrong with sign out ${userResponse.Message}");
                        return;
                    }

                    /* The user successfully logged out */
                    /* Save new data to the registry */
                    _config.SetConfigValue("userId", "");
                    _config.SetConfigValue("userEmail", "");
                    _config.SetConfigValue("userName", "");
                    _config.SetConfigValue("userToken", "");
                    _config.SetConfigValue("loggedInTimeStamp", "");
                    _config.SetConfigValue("isLoggedIn", "false");

                    MessageBox.Show($"Goodbye :(");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Whoops! Something went wrong during sign out! {ex.Message}");
            }
        }
    }
}
