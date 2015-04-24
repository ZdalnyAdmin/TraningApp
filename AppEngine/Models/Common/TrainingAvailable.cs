
namespace AppEngine.Models.Common
{
    public class TrainingAvailable
    {
        public int TrainingAvailableID { get; set; }
        public int TrainingID { get; set; }
        public bool IsAvailableForAll { get; set; }
        public int ProfileGroupID { get; set; }
    }
}
