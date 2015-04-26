using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.DataObject
{
    public class AppSetting
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AppSettingID { get; set; }
        public bool AllowUserToChangeName { get; set; }
        public bool AllowUserToChangeMail { get; set; }
    }
}
