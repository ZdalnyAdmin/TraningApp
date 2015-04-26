using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.Common
{
    public class ProfileGroup2Person
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProfileGroup2PersonID { get; set; }
        public int ProfileGroupID { get; set; }
        public ProfileGroup ProfileGroup { get; set; }
        public int PersonID { get; set; }
        public Person Person { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUserID { get; set; }
    }
}
