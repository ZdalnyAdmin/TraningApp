using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Models.DataBusiness
{
    public enum StatusEnum
    {
        [Description("Aktywny")]
        Active = 1,

        [Description("Zablokowany")]
        Blocked = 2,

        [Description("Usunięty")]
        Deleted = 3,

        [Description("Zaproszony")]
        Invited = 4,

        [Description("Odrzucony")]
        Rejected = 5
    }
}
