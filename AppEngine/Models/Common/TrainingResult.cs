using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.Common
{
    public class TrainingResult
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)] 
        public int TrainingResultID { get; set; }

        public int TrainingID { get; set; }
        public Training Training { get; set; }
        public int PersonID { get; set; }
        public Person Person { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Rating { get; set; }
        public bool IsCompleted
        {
            get { return EndDate.HasValue; }
        }
    }
}
