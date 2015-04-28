using AppEngine.Models;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers.Api
{
    public class LogsController : ApiController
    {
        private EFContext db = new EFContext();

        [HttpGet]
        public IEnumerable<AppLog> Get()
        {
            //get from correct profil
            var logs = db.Logs.ToList();

            foreach(var item in logs)
            {
                if(!string.IsNullOrWhiteSpace(item.PersonID))
                {
                    item.Person = db.Users.FirstOrDefault(x => x.Id == item.PersonID);
                }

                if (!string.IsNullOrWhiteSpace(item.ModifiedUserID))
                {
                    item.ModifiedUser = db.Users.FirstOrDefault(x => x.Id == item.ModifiedUserID);
                }

                if (item.TrainingID != 0)
                {
                    item.Training = db.Trainings.FirstOrDefault(x => x.TrainingID == item.TrainingID);
                }
            }

            return logs;
        }
    }
}
