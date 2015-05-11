using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Models.DataBusiness
{
    public enum TrainingManagmentActionType
    {
        GetInternal = 0,
        GetExternal = 1,
        GetSettings = 2,
        GetGroups = 3
    }

    public enum UserManagmentActionType
    {
        Get = 0,
        Delete = 1,
        Edit = 2,
    }
}
