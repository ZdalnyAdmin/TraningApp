
namespace OrganizationModule.Models
{
    public class Statistic
    {
        public int PersonNumber { get; set; }
        public int ActivePersonNumber { get; set; }
        public int BlockedPersonNumber { get; set; }

        public int DeleteAccountNumber { get; set; }
        public int StartTraningNumber { get; set; }
        public int CompleteTraningNumber { get; set; }

        public int ActivePersonInLastWeek { get; set; }
        public int ActivePersonInLastMonth { get; set; }


        public static Statistic Get()
        {
            var obj = new Statistic();
            obj.PersonNumber = 10;
            obj.ActivePersonNumber = 5;
            obj.BlockedPersonNumber = 2;

            obj.DeleteAccountNumber = 2;
            obj.StartTraningNumber = 4;
            obj.CompleteTraningNumber = 5;

            obj.ActivePersonInLastWeek = 5;
            obj.ActivePersonInLastMonth = 6;
            return obj;
        }
    }
}