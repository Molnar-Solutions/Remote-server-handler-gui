using MD_Networking.Exceptions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD_Store.Lib
{
    public class RegistryConfig
    {
        private RegistryConfig() {}

        public bool SetConfigValue(string key, string value)
        {
            try
            {
                RegistryKey _appKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\ServerHandler");
                _appKey.SetValue(key, value);
                _appKey.Close();
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        public void RemoveConfigDir(string folder)
        {
            try
            {
                 Registry.CurrentUser.DeleteSubKey($@"SOFTWARE\ServerHandler\{folder}");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool SetConfigValue(string folder, string key, string value)
        {
            try
            {
                RegistryKey _appKey = Registry.CurrentUser.CreateSubKey($@"SOFTWARE\ServerHandler\{folder}");
                _appKey.SetValue(key, value);
                _appKey.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static RegistryConfig Init()
        {
            return new RegistryConfig();
        }

        public string? GetConfigValue(String name)
        {
            try
            {
                RegistryKey _appKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\ServerHandler");
                var value = _appKey.GetValue($"{name}").ToString();
                _appKey.Close();
                return value;
            } catch (Exception ex)
            {
                return "";
            }
        }

        public string? GetConfigValue(string folder, string name)
        {
            try
            {
                RegistryKey _appKey = Registry.CurrentUser.CreateSubKey($@"SOFTWARE\ServerHandler\{folder}");

                if (_appKey.GetValue(name) is not null)
                {
                    return _appKey.GetValue(name).ToString();
                }

                _appKey.Close();
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
