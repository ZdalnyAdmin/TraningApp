using System;
using System.Globalization;
using System.Web.Configuration;

namespace AppEngine.Helpers
{
    public static class AppSettings
    {
        public static T Setting<T>(string name)
        {
            string value = WebConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception(String.Format("Could not find setting '{0}',", name));
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
