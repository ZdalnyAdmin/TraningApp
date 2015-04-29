using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Models.DataBusiness
{
    public enum ProfileEnum
    {
        [Description("Superuser")]
        Superuser = 0,

        [Description("Administrator")]
        Administrator = 1,

        [Description("Manager")]
        Manager = 2,

        [Description("Twórca")]
        Creator = 3,

        [Description("Opiekun")]
        Keeper = 4,

        [Description("Uzytkownik")]
        User = 5
    }
}
