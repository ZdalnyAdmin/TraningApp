using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;

namespace AppEngine.ViewModels
{
    public class BaseViewModel
    {
        public Person LoggedUser { get; set; }
        public Organization CurrentOrganization { get; set; }
        public string ErrorMessage { get; set; }
        public BaseActionType ActionType { get; set; }
    }
}
