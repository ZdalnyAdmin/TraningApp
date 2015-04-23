using System;

namespace AppEngine.Models
{
    public class Organization
    {
        public int OrganizationID { get; set; }
        public string Name { get; set; }
        public decimal SpaceDisk { get; set; }
        public int MaxAssignedUser { get; set; }
        public bool IsGlobalAvailable { get; set; }
        public bool IsTraningAvailableForAll { get; set; }
        public bool CanUserChangeMail { get; set; }
        public bool CanUserChangeName { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserID { get; set; }
        public int PatronID { get; set; }
        public int StatusID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedDate { get; set; }
        public int DeletedUserID { get; set; }
    }
}
