using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.ViewModels
{
    public class TrainingManagmentViewModel
    {
        public Person LoggedUser { get; set; }
        public Organization CurrentOrganization { get; set; }
        public bool ShowExternal { get; set; }
        public List<Training> InternalTrainings { get; set; }
        public List<Training> ExternalTrainings { get; set; }
        public List<ProfileGroup> Groups { get; set; }
        public Training Current { get; set; }

        public TrainingManagmentActionType ActionType { get; set; }
    }


}
