using System;
using System.Globalization;
using System.IO;
using System.Web.Configuration;
using System.Linq;

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

        private static string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string ServerPath()
        {
            return Path.Combine("Assets", AppSettings.generateID(), "Resources");
        }

        public static string CopyFile(string filePath,string domainPath, string sourcePath, out decimal size, bool delete=true)
        {
            try
            {
                size = 0;

                if (sourcePath.StartsWith("/"))
                {
                    sourcePath = sourcePath.Substring(1, sourcePath.Length-1);
                    sourcePath = sourcePath.Replace("/", "\\");
                }

                sourcePath = Path.Combine(domainPath, sourcePath);

                var name = System.IO.Path.GetFileName(sourcePath);

                if (name.Contains("_"))
                {
                    var temp = (from t in name.Split('_')
                                select t).Skip(1);

                    name = string.Join("_", temp);
                }

                var destFile = Path.Combine(filePath, name);

                System.IO.File.Copy(sourcePath, destFile, true);

                var toDelete = sourcePath.Contains("Temp");

                if (toDelete)
                {
                    File.Delete(sourcePath);
                }

                FileInfo f = new FileInfo(destFile);
                long s1 = f.Length;

                if (s1 != 0)
                {
                    size = System.Convert.ToDecimal((s1 / 1024f) / 1024f);
                }

                return destFile.Replace(domainPath, "");
            }
            catch (Exception ex)
            {
                size = 0;
                return string.Empty;
            }

        }
    }
}
