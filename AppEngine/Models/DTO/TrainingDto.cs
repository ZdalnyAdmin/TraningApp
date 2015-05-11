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
        public float Result { get; set; }
        public bool IsComplete
        {
            get { return EndDate.HasValue; }
        }
    }
}
