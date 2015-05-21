using AppEngine.Models;
using AppEngine.Models.DataBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.ViewModels
{
    public class LogsViewModel
    {
        public List<AppLog> Logs { get; set; }
        public List<AppLog> DisplayLogs { get; set; }
        public List<EnumData> Criteria { get; set; }
        public string Success { get; set; }
    }
}
