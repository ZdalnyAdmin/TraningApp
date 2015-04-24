using System;

namespace AppEngine.Models.Common
{
    public class TrainingResult
    {
        public int TrainingResultID { get; set; }
        public int TrainingID { get; set; }
        public int PersonID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Rating { get; set; }
        public bool IsCompleted
        {
            get { return EndDate.HasValue; }
        }
    }
}
