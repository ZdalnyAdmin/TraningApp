using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppEngine.Models.ViewModels.Upload
{
    public class DeleteViewModel
    {
        [Required]
        public string FileName { get; set; }
    }
}
