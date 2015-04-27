using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Models.DataBusiness
{
    public class Statistic
    {
        public int ActivePeople { get; set; }
        public int BlockedPeople { get; set; }

        public int DeleteAccount { get; set; }

        public int StartedTrainings { get; set; }
        public int CompletedTrainings { get; set; }

        public int WeekActiveUser { get; set; }
        public int MonthActiveUser { get; set; }

        public int People
        {
            get { return ActivePeople + BlockedPeople; }
        }
    }
}
