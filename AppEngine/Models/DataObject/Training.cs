using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppEngine.Models.DataObject;
using AppEngine.Models.DataBusiness;

namespace AppEngine.Models.Common
{
    public class Training
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TrainingID { get; set; }
        public string Name { get; set; }
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
        /// <summary>
        /// Get from training to organization ?? create ??
        /// </summary>
        [NotMapped]
        public string AssignedOrganizationsName { get; set; }
        /// <summary>
        /// Get from training to organization ?? create ??
        /// </summary>
        [NotMapped]
        public string AssignedOrganizationsID { get; set; }
        /// <summary>
        /// Get from logs
        /// </summary>
        [NotMapped]
        public DateTime LastActivationDate { get; set; }

        //logo szkolenia - sciezka
        public string TrainingResources { get; set; }
        //liczba punktow zeby zdac
        public int PassResult { get; set; }
        //tekst jak zaliczone szkolenie
        public string PassInfo { get; set; }
        //odznaka - sciezka do pliku
        public string PassResources { get; set; }

        [NotMapped]
        public List<SimpleObject> Logs { get; set; }
        [NotMapped]
        public List<SimpleObject> AssignedGroups { get; set; }


        private string _createUserName;
        public string CreateUserName
        {
            get { return _createUserName; }
        }
        public void SetCreateUserName(string name)
        {
            _createUserName = name;
        }

        private int _runTrainingStats;
        public int RunTrainingStats
        {
            get { return _runTrainingStats; }
        }
        public void SetRunTrainingStats(int value)
        {
            _runTrainingStats = value;
        }

    }

    public enum TrainingType
    {
        Internal = 0,
        Kenpro = 1, 
        Other = 2,
    }
}
