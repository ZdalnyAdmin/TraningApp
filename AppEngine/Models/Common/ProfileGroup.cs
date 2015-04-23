using System;

namespace AppEngine.Models.Common
{
    public class ProfileGroup
    {
        public int ProfileGroupID { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int DeletedUserID { get; set; }
    }
}
