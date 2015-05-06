using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
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
        public string CreateUserID { get; set; }
        public string ProtectorID { get; set; }
        public OrganizationEnum Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedUserID { get; set; }
        public string DeletedReason { get; set; }

        [NotMapped]
        public int AssignedUser { get; set; }
        [NotMapped]
        public int BlockedUser { get; set; }
        [NotMapped]
        public int DeleteUser { get; set; }
        [NotMapped]
        public int InvationUser { get; set; }
        [NotMapped]
        public Person Protector { get; set; }
        [NotMapped]
        public decimal UsedSpaceDisk { get; set; }

    }
}
