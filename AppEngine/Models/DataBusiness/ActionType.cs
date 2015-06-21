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
        SaveGroups = 3
    }

    public enum BaseActionType
    {
        Get = 0,
        Delete = 1,
        Edit = 2,
        Add = 3,
        GetSimple = 4,
        GetExtData = 5,
        ById = 6,
        GetSpecial = 7,
        SaveProtector = 8,
        GetByCreateUser = 9,
        ChangeImage = 10,
    }

    public enum PeopleActionType
    {
        GetProtectors = 0,
        DeleteProtector = 1,
        EditProtector = 2,
        AddProtector = 3,
        GetAdministrators = 5,
    }
}
