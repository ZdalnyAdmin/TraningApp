using AppEngine.Models;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public IEnumerable<LogsViewModel> Get()
        {
            //get from correct profil
            var logs = db.Logs.Where(x => x.IsSystem).OrderByDescending(x=>x.ModifiedDate).ToList();
            foreach (var item in logs)
            {
                if (!string.IsNullOrWhiteSpace(item.ModifiedUserID))
                {
                    item.ModifiedUser = db.Users.FirstOrDefault(x => x.Id == item.ModifiedUserID);
                }
            }

            var obj = new LogsViewModel();
            obj.Logs = logs;
            obj.DisplayLogs = logs;
            obj.Criteria = new List<EnumData>();

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

                obj.Criteria.Add(data);
                index++;
            }
            //return descs;
            obj.Success = String.Empty;

            var list = new List<LogsViewModel>();
            list.Add(obj);

            return list;
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
