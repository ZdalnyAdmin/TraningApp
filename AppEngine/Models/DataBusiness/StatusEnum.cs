using System.ComponentModel;

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

        [Description("Oczekujący")]
        Invited = 4,

        [Description("Nieaktualne - wysłano kolejne")]
        Reinvited = 5,

        [Description("Usunięte")]
        Rejected = 6
    }
}
