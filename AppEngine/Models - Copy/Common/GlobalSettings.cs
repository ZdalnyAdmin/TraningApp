using System;

namespace AppEngine.Models
{
    public class GlobalSettings
    {
        public int GlobalSettingsID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int MofifiedUserID { get; set; }
    }
}
