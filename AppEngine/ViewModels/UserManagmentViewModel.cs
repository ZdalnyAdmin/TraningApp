using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using System.Collections.Generic;

namespace AppEngine.ViewModels
{
    public class UserManagmentViewModel : BaseViewModel
    {
        public List<Person> People { get; set; }
        public List<Person> DeletedPeople { get; set; }
        public Person Current { get; set; }

        public int PeopleCount { get; set; }

        public BaseActionType ActionType { get; set; }
    }
}
