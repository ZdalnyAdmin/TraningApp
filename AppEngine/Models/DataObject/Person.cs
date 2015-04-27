using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.Common
{
    public class Person
    {
        private int _trainingNumber;
        private string _assignedGroups;

        #region Properties

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)] 
        public int PersonID { get; set; }
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }

        public string Name { get; set; }
        public string Mail { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int StatusID { get; set; }
        public Status Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RegistrationUserID { get; set; }
        public DateTime? LastActivationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeleteUserID { get; set; }
        public int? OrganizationID { get; set; }
        public Organization Organization { get; set; }

        //public List<TrainingResult> AssignedTrainings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TrainingNumber
        {
            get { return _trainingNumber; }
        }

        public string AssignedGroups
        {
            get { return _assignedGroups; }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        public void SetAssignedTrainingsNumber(int number)
        {
            _trainingNumber = number;
        }

        public void SetAssignedGroups(List<string> groups)
        {
            if(groups != null && groups.Any())
            {
                _assignedGroups = String.Join(",", groups);
            }
        }

        #endregion Methods
    }
}
