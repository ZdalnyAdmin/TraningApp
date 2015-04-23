using System;

namespace AppEngine.Models.Common
{
    public class TraningResult
    {
        public int TraningResultID { get; set; }
        public int TraningID { get; set; }
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
