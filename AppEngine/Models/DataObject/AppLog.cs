using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models
{
    public class AppLog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AppLogID { get; set; }
        public bool IsSystem { get; set; }
        public OperationLog OperationType { get; set; }
        public SystemLog SystemType { get; set; }
        public bool IsSystemLog { get; set; }
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
        public string ModifiedUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [NotMapped]
        public Person ModifiedUser { get; set; }
        public int? TrainingID { get; set; }
        [NotMapped]
        public Training Training { get; set; }
        public string PersonID { get; set; }
        [NotMapped]
        public Person Person { get; set; }
        public string OrganizationName { get; set; }
        public int OrganizationID { get; set; }
    }




}
