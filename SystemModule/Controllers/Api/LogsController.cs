using AppEngine.Models;
using AppEngine.Models.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class LogsController : ApiController
    {
        private EFContext db = new EFContext();

        [HttpGet]
        public IEnumerable<AppLog> Get()
        {
            //get from correct profil
            var logs = db.Logs.Where(x => x.IsSystem).ToList();
            foreach (var item in logs)
            {
                if (!string.IsNullOrWhiteSpace(item.ModifiedUserID))
                {
                    item.ModifiedUser = db.Users.FirstOrDefault(x => x.Id == item.ModifiedUserID);
                }
            }

            //var descs = new List<EnumData>();
            //var type = typeof(SystemLog);
            //var names = Enum.GetNames(type);
            //int index = 1;
            //EnumData data = null;
            //foreach (var name in names)
            //{
            //    data = new EnumData();
            //    var field = type.GetField(name);
            //    var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            //    foreach (DescriptionAttribute fd in fds)
            //    {
            //        data.Name = fd.Description;
            //        data.Type = index;
            //    }

            //    descs.Add(data);
            //    index++;
            //}
            //return descs;


            return logs;
        }

        // POST api/<controller>
        public HttpResponseMessage Post(AppLog log)
        {

            if (ModelState.IsValid)
            {
                var list = (from t in db.Logs
                            where t.SystemType == log.SystemType && t.TrainingID == log.TrainingID && log.IsSystem
                            orderby t.ModifiedDate descending
                            select t).ToList();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, list);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}
