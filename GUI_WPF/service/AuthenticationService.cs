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
                string? registryValue = _config.GetConfigValue($"{App.ACTIVE_USER_ID}", "isLoggedIn");
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

                    /* Set the active user */
                    App.ACTIVE_USER_ID = userData.id;

                    /* Save response data to the registry */
                    /* set active user data in a different folder */
                    _config.SetConfigValue($"{userData.id}", "userId", $"{userData.id}");
                    _config.SetConfigValue($"{userData.id}", "userEmail", $"{userData.email}");
                    _config.SetConfigValue($"{userData.id}", "userName", $"{userData.name}");
                    _config.SetConfigValue($"{userData.id}", "userToken", $"{userData.token}");
                    _config.SetConfigValue($"{userData.id}", "isLoggedIn", "true");
                    /* Token is active for 8 hours, if the time gap is longer than I remove all the data that I set into the registry */
                    _config.SetConfigValue($"{userData.id}", "loggedInTimeStamp", $"{DateTime.Now.ToString()}");

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
                string? registryValue = _config.GetConfigValue($"{App.ACTIVE_USER_ID}", "isLoggedIn");
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

                string loggedInTimeStamp = _config.GetConfigValue($"{App.ACTIVE_USER_ID}", "loggedInTimeStamp");

                if (string.IsNullOrEmpty(loggedInTimeStamp))
                {
                    /* The system do not save the logged in timestamp so remove everything */
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userId", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userEmail", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userName", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userToken", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "loggedInTimeStamp", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "isLoggedIn", "false");
                    _config.RemoveConfigDir($"{App.ACTIVE_USER_ID}");
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
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userId", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userEmail", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userName", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userToken", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "loggedInTimeStamp", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "isLoggedIn", "false");
                    _config.RemoveConfigDir($"{App.ACTIVE_USER_ID}");
                    return;
                }

                string userToken = _config.GetConfigValue($"{App.ACTIVE_USER_ID}", "userToken");

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
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userId", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userEmail", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userName", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "userToken", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "loggedInTimeStamp", "");
                    _config.SetConfigValue($"{App.ACTIVE_USER_ID}", "isLoggedIn", "false");
                    _config.RemoveConfigDir($"{App.ACTIVE_USER_ID}");

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
