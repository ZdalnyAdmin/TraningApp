using System;

namespace AppEngine.Models
{
    public class AppLog
    {
        public int AppLogID { get; set; }
        public int OperationType { get; set; }
        public int ModifiedUserID { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int TrainingID { get; set; }
        public int PersonID { get; set; }
    }
}
