using System;

namespace AppEngine.Models.Common
{
    public class ProfileGroup2Person
    {
        public int ProfileGroupID { get; set; }
        public int PersonID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedDate { get; set; }
        public int DeletedUserID { get; set; }
    }
}
