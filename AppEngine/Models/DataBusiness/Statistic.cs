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

        public int OrganizatonsTrainings {get;set;}
        public int StartedTrainings { get; set; }
        public int CompletedTrainings { get; set; }
        public int InternalTrainings { get;set;}
       

        public int WeekActiveUser { get; set; }
        public int MonthActiveUser { get; set; }

        //new
        public string Name { get; set; }
        public DateTime CreateDate { get;set;} 

        public int PeopleNo { get;set;}
        public int OrganizationNo { get;set;}
        public int PeoplaInOrganizationAverage 
        { 
            get
            {
                if(OrganizationNo != 0)
                {
                    return PeopleNo / OrganizationNo;
                }
                return PeopleNo;
            }
            
        }

        public int People
        {
            get { return ActivePeople + BlockedPeople; }
        }
    }
}
