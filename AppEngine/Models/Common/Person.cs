using System;
using System.Collections.Generic;

namespace AppEngine.Models.Common
{
    public class Person
    {
        #region Properties

        public int PersonID { get; set; }
        public int ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastActivationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int DeletedUserID { get; set; }
        public Person DeleteUser { get; set; }
        public int OrganizationID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TraningNumber 
        {
            get { return AssignedTranings != null ? AssignedTranings.Count : 0; }
        }

        public List<TraningResult> AssignedTranings { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}
