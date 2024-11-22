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

        /*** I have to close it after! */
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
        public static RegistryConfig Init()
        {
            return new RegistryConfig();
        }

        /*** I have to close it after! */
        public string GetConfigValue(String name)
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
    }
}
