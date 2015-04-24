using System;
using System.Collections.Generic;

namespace AppEngine.Models.Common
{
    public class Person
    {
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
        public DateTime DeletedDate { get; set; }
        public int DeletedUserID { get; set; }
        public int OrganizationID { get; set; }

        //
        public string Registration
        {
            get
            {
                return RegistrationDate != null ? RegistrationDate.ToString("dd.MM.yyyy hh.mm") : String.Empty;
            }
        }
        public string LastActivation 
        {
            get
            {
                return LastActivationDate != null ? LastActivationDate.ToString("dd.MM.yyyy hh.mm") : String.Empty;
            }
        }
        public int TraningNumber 
        {
            get { return AssignedTranings != null ? AssignedTranings.Count : 0; }
        }
        public List<Traning> AssignedTranings { get; set; }
    }
}
