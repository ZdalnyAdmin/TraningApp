using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AppEngine.Models.Common
{
    public class ProfileGroup
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProfileGroupID { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUserID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedUserID { get; set; }
        //public Person DeletedUser { get; set; }

        [NotMapped]
        public string AssignedPeopleDisplay
        {
            get
            {
                if (AssignedPeople != null && AssignedPeople.Any())
                {
                    return String.Join(",", (from t in AssignedPeople
                                             select t.DisplayName).ToList());
                }

                return string.Empty;
            }
        }

        [NotMapped]
        public string DisplayID
        {
            get
            {
                return string.Format("{0}_{1}", ProfileGroupID, Name);
            }
        }

        [NotMapped]
        public List<Person> AssignedPeople { get; set; }

    }
}
