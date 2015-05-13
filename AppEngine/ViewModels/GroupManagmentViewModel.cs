using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using System.Collections.Generic;

namespace AppEngine.ViewModels
{
    public class GroupManagmentViewModel : BaseViewModel
    {
        public List<ProfileGroup> Groups { get; set; }
        public ProfileGroup Current { get; set; }
        public BaseActionType ActionType { get; set; }

    }
}
