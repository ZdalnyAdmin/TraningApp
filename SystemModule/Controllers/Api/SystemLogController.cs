using AppEngine.Models;
using AppEngine.Models.DataBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class SystemLogController : ApiController
    {
        [HttpGet]
        public IEnumerable<EnumData> Get()
        {
            var descs = new List<EnumData>();
            var type = typeof(SystemLog);
            var names = Enum.GetNames(type);
            int index = 1;
            EnumData data = null;
            foreach (var name in names)
            {
                data = new EnumData();
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    data.Name = fd.Description;
                    data.Type = index;
                }

                descs.Add(data);
                index++;
            }
            return descs;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

}
