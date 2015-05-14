using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        public List<Person> People { get; set; }
        public PeopleActionType ActionType { get; set; }
        public Person Current { get; set; }
    }
}
