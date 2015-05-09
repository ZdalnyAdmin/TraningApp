using AppEngine.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.DataObject
{
    public class Person2Organization
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Person2OrganizationID { get; set; }
        public string PersonID { get; set; }
        public Person Person { get; set; }
        public int OrganizationID { get; set; }
        public Organization Organization { get; set; }
    }
}
