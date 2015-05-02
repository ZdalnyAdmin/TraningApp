using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Models.DataBusiness
{
    public class UserImage
    {
        public Stream ImageSource { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string OrganizationName { get; set; }
        public UserImageEnum ImageType { get; set; }
        public bool LoadData { get; set; }
        public bool SaveData { get; set; }
    }

    public enum UserImageEnum
    {
        Mark = 0,
        Logo = 1,
    }
}
