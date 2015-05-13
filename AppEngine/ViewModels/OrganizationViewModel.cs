using AppEngine.Models.Common;
using AppEngine.Models.DataObject;
using AppEngine.Models.DTO;
using System.Collections.Generic;

namespace AppEngine.ViewModels
{
    public class OrganizationViewModel : BaseViewModel
    {
        public Organization Current { get; set; }
        public List<Organization> Organizations { get; set; }
        public AppSetting Setting { get; set; }
        public List<OrganizationDto> NotAssigned { get; set; }
        public int OrganizationID { get; set; }
        public OrganizationDto Detail { get; set; }
        public Person Protector { get; set; }
    }
}
