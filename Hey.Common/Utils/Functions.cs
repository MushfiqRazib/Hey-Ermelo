using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.Common.Utils
{
    public class Functions
    {
        public static string GetValueFromWebConfig(string key)
        {
            try
            {
                return System.Configuration.ConfigurationSettings.AppSettings[key].ToString();
            }
            catch
            {
                throw new Exception("Key '" + key + "' is not found in the web.config file.");
            }
        }
    }
}
