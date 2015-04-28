using AppEngine.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace AppEngine.Models
{
    public class AppLog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AppLogID { get; set; }
        public OperationLog OperationType { get; set; }
        [NotMapped]
        public string OperationDesc
        {
            get
            {
                var type = typeof(OperationLog);
                var name = Enum.GetName(type, OperationType);
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    return fd.Description;
                }
                return String.Empty;
            }
        }
        public int ModifiedUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [NotMapped]
        public Person ModifiedUser { get; set; }
        public int? TrainingID { get; set; }
        [NotMapped]
        public Training Training { get; set; }
        public int? PersonID { get; set; }
        [NotMapped]
        public Person Person { get; set; }
    }


    public enum OperationLog
    {
        [Description("Zaproszenie uzytkownika")]
        Zaproszenie = 0,
        [Description("Samousuniecie uzytkownika")]
        Samousuniecie = 1,
        [Description("Usuniecie uzytkownika")]
        Usuniecie = 2,
        [Description("Utworzenie kursu")]
        KursNowy = 3,
        [Description("Utworzenie uzytkownika")]
        Uzytkownik = 4,
        [Description("Edycja uzytkownika")]
        Edycja = 5,
        [Description("Edycja szkolenia")]
        KursEdycja = 6,
    }

}
