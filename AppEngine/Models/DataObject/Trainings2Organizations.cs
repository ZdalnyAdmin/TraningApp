using AppEngine.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.DataObject
{
    public class Trainings2Organizations
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Trainings2OrganizationsID { get; set; }
        public int OrganizationID { get; set; }
        public Organization Organization { get; set; }
        public int TrainingID { get; set; }
        public Training Training { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedUserID { get; set; }
    }
}
