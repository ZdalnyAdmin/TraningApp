using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.Common
{
    public class Status
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)] 
        public int StatusID { get; set; }
        public string Name { get; set; }
    }
}
