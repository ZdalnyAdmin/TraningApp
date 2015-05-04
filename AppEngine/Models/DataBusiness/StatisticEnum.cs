using System.ComponentModel;

namespace AppEngine.Models.DataBusiness
{
    public enum StatisticEnum
    {
        [Description("Organizacje")]
        Organization = 1,

        [Description("Kursy globalne")]
        Global = 2,

        [Description("Ogolne")]
        General = 3,
    }
}
