using System;
using System.Globalization;
using System.IO;
using System.Web.Configuration;
using System.Linq;
using AppEngine.Models.DataContext;

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

        public static string CopyTrainingImages(string filePath, string domainPath, string sourcePath, out decimal size, bool delete = true)
        {
            try
            {
                size = 0;

                if (sourcePath.StartsWith("/"))
                {
                    sourcePath = sourcePath.Substring(1, sourcePath.Length - 1);
                    sourcePath = sourcePath.Replace("/", "\\");
                }

                sourcePath = Path.Combine(domainPath, sourcePath);
                var name = System.IO.Path.GetFileName(sourcePath);
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
                var destFile = Path.Combine(filePath, name);

                //na razie zapisuje do sciezki na dysku dodatkowo
                var localPath = filePath.Replace(domainPath, "C:\\");
                localPath = localPath.Replace("\\Resources", string.Empty);

                if(!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                localPath = Path.Combine(localPath, name);
                //copy to local
                System.IO.File.Copy(sourcePath, localPath, true);
                var toDelete = sourcePath.Contains("Temp");
                if (toDelete)
                {
                    File.Delete(sourcePath);
                }

                FileInfo f = new FileInfo(localPath);
                long s1 = f.Length;

                if (s1 != 0)
                {
                    size = System.Convert.ToDecimal((s1 / 1024f) / 1024f);
                }

                return localPath.Replace("C:\\Assets", "File");
            }
            catch (Exception ex)
            {
                size = 0;
                return string.Empty;
            }

        }

        public static decimal GetUsedSpace(EFContext context, int organizationID)
        {

            var trainings = (from tio in context.TrainingsInOrganizations
                             join t in context.Trainings on tio.TrainingID equals t.TrainingID
                             join td in context.TrainingDetails on t.TrainingID equals td.TrainingID
                             where tio.OrganizationID == organizationID
                             select td).ToList();

            return trainings.Sum(x => x.FileSize);
        }
    }
}
