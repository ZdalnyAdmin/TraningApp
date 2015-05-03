using AppEngine.Models.DataBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppEngine.Services
{
    public class ImageService
    {
        private const string DIRECTORY_ASSETS = "Assets";
        private const string DIRECTORY_MARKS = "Marks";
        private const string DIRECTORY_ALL = "All";


        public static List<string> LoadImage(UserImage obj)
        {
            var path = String.Empty;
            var localPath = Path.Combine(DIRECTORY_ASSETS, DIRECTORY_MARKS);
            var files = new List<string>();

            switch (obj.ImageType)
            {
                case UserImageEnum.Mark:
                    if (String.IsNullOrEmpty(obj.OrganizationName))
                    {
                        //get for all
                        localPath = Path.Combine(localPath, DIRECTORY_ALL);
                        path = Path.Combine(HttpRuntime.AppDomainAppPath, localPath);
                    }



                    if (Directory.Exists(path))
                    {
                        files = Directory.GetFiles(path).ToList();
                    }

                    var temp = new List<string>();

                    foreach (var item in files)
                    {

                        temp.Add(item.Remove(0, HttpRuntime.AppDomainAppPath.Length));
                    }

                    files = temp;

                    break;
                case UserImageEnum.Logo:
                    break;
                default:
                    break;
            }

            return files;
        }

        public static void SaveImage()
        {

        }

    }
}
