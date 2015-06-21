using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Models.DTO
{
    public class TrainingDto : CommonDto
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Organization { get; set; }
        public float Result { get; set; }
        public string CreatorName { get; set; }
        public string CreatorLogin { get; set; }
        public List<string> AssignedGroup { get; set; }
        public string CreatorID { get; set; }
        public bool IsComplete
        {
            get { return EndDate.HasValue; }
        }
    }
}
