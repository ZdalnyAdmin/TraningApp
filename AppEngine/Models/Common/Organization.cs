using AppEngine.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.Common
{
    public class Organization
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)] 
        public int OrganizationID { get; set; }
        public string Name { get; set; }
        public decimal SpaceDisk { get; set; }
        public int MaxAssignedUser { get; set; }
        public bool IsGlobalAvailable { get; set; }
        public bool IsTrainingAvailableForAll { get; set; }
        public bool CanUserChangeMail { get; set; }
        public bool CanUserChangeName { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserID { get; set; }
        //public Person CreateUser { get; set; }
        public int? ProtectorID { get; set; }
        //public Person Protector { get; set; }
        public int StatusID { get; set; }
        //public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUserID { get; set; }
       //public Person DeletedUser { get; set; }
    }
}
