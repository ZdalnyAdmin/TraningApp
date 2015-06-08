using AppEngine.Models.DataObject;
using AppEngine.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.Common
{
    public class Training
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TrainingID { get; set; }
        [MinLength(5)]
        [MaxLength(80)]
        [Required]
        public string Name { get; set; }
        [MinLength(5)]
        [MaxLength(1500)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public TrainingType TrainingType { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUserID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifieddUserID { get; set; }

        public List<TrainingDetail> Details { get; set; }
        public List<TrainingQuestion> Questions { get; set; }

        [NotMapped]
        public List<ProfileGroup> Groups { get; set; }
        [NotMapped]
        public List<Organization> Organizations { get; set; }
        [NotMapped]
        public DateTime LastActivationDate { get; set; }

        //logo szkolenia - sciezka
        public string TrainingResources { get; set; }
        //liczba punktow zeby zdac
        public Int32? PassResult { get; set; }
        //tekst jak zaliczone szkolenie
        [MinLength(5)]
        [MaxLength(500)]
        public string PassInfo { get; set; }
        //odznaka - sciezka do pliku
        public string PassResources { get; set; }

        [NotMapped]
        public List<CommonDto> Logs { get; set; }
        [NotMapped]
        public List<CommonDto> AssignedGroups { get; set; }
        [NotMapped]
        public string UserName { get; set; }
    }

    public enum TrainingType
    {
        Internal = 0,
        Kenpro = 1, 
        Other = 2,
    }
}
