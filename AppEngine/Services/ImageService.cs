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
            var files = new List<string>();

            switch (obj.ImageType)
            {
                case UserImageEnum.Mark:
                    if (String.IsNullOrEmpty(obj.OrganizationName))
                    {
                        //get for all
                        path = Path.Combine(HttpRuntime.AppDomainAppPath, DIRECTORY_ASSETS, DIRECTORY_MARKS, DIRECTORY_ALL);
                    }



                    if (Directory.Exists(path))
                    {
                        files = Directory.GetFiles(path).ToList();
                    }


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
