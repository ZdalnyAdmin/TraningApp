using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AppEngine.Models.Common
{
    public class ProfileGroup
    {
        private string _assignedPeople;

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)] 
        public int ProfileGroupID { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUserID { get; set; }
        //public Person DeletedUser { get; set; }


        public string AssignedPeople
        {
            get { return _assignedPeople; }
        }

        public void SetAssignedPeople(List<string> people)
        {
            if (people != null && people.Any())
            {
                _assignedPeople = String.Join(",", people);
            }
        }

    }
}
