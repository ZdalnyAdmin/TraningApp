using System;

namespace AppEngine.Models.Common
{
    public class Training
    {

        public int TrainingID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int TrainingTypeID { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserID { get; set; }
    }
}
