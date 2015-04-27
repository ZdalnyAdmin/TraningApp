using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AppEngine.Models.Common
{
    public class ProfileGroup
    {
        private List<Person> _assignedPeople;

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProfileGroupID { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUserID { get; set; }
        //public Person DeletedUser { get; set; }

        public string AssignedPeopleDisplay
        {
            get
            {
                if (_assignedPeople != null && _assignedPeople.Any())
                {
                    return String.Join(",", (from t in _assignedPeople
                                                           select t.Name).ToList());
                }  
 
                return string.Empty;
            }
        }

        public void SetAssignedPeople(List<Person> people)
        {
            _assignedPeople = people;
        }

    }
}
