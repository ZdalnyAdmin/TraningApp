using AppEngine.Models.DataBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Helpers
{
    public class AccessAttribute : Attribute
    {
        public ProfileEnum MinimumProfile { get; set; }

        public AccessAttribute(ProfileEnum minimumProfile = ProfileEnum.User)
        {
            MinimumProfile = minimumProfile;
        }
    }
}
