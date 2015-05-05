using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Models.DataBusiness
{
    public class Result
    {
        public bool Succeeded {get; set;}
        public List<string> Errors { get; set; }
        public string Message { get; set; }
    }
}
