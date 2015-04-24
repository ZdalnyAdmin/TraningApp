
namespace AppEngine.Models.Common
{
    public class TraningAvailable
    {
        public int TraningAvailableID { get; set; }
        public int TraningID { get; set; }
        public bool IsAvailableForAll { get; set; }
        public int ProfileGroupID { get; set; }
    }
}
