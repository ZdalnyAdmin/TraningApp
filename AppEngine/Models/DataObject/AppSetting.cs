using AppEngine.Models.Common;
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
        public int? ProtectorID { get; set; }
        [NotMapped]
        public Person Protector { get; set; }
        public int SpaceDisk { get; set; }
        public int MaxAssignedUser { get; set; }
        public bool IsGlobalAvailable { get; set; }
        public bool IsTrainingAvailableForAll { get; set; }
        public int MaxActiveTrainings { get; set; }
        public bool IsDefault { get; set; }
        public string DefaultName { get; set; }
        public string DefaultEmail { get; set; }
    }
}
