using AppEngine.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.DataObject
{
    public class ProfileGroup2Trainings
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProfileGroup2TrainingID { get; set; }
        public int ProfileGroupID { get; set; }
        public ProfileGroup ProfileGroup { get; set; }
        public int TrainingID { get; set; }
        public Training Training { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUserID { get; set; }
    }
}
