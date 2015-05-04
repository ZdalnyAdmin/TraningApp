using System.ComponentModel;

namespace AppEngine.Models.DataBusiness
{
    public enum StatisticEnum
    {
        [Description("Ogolne")]
        General = 1,

        [Description("Organizacje")]
        Organization = 2,

        [Description("Kursy globalne")]
        Global = 3,
    }
}
