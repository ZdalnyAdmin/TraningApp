using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using System;
using System.Linq;
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
        public string NewName { get; set; }
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
        public string SecurityStamp { get; set; }
        public DateTime? ModifiedDate { get; set; }

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

        public void UpdateSecurityStamp()
        {
            this.SecurityStamp = Guid.NewGuid().ToString();
        }

        public string GenerateToken(string purpose)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(this.OrganizationID + purpose + this.SecurityStamp);
            plainTextBytes = plainTextBytes.Reverse().ToArray();

            return System.Convert.ToBase64String(plainTextBytes);
        }

        public bool IsTokenValid(string purpose, string token)
        {
            var newToken = this.GenerateToken(purpose);
            return newToken.Equals(token, StringComparison.InvariantCulture);
        }

    }
}
