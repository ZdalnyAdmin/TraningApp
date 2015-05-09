using AppEngine.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.DataObject
{
    public class ProfileGroup2Organization
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProfileGroup2OrganizationID { get; set; }
        public int ProfileGroupID { get; set; }
        public ProfileGroup ProfileGroup { get; set; }
        public int OrganizationID { get; set; }
        public Organization Organization { get; set; }
    }
}
