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
                if(item.PersonID != 0)
                {
                    item.Person = db.Persons.FirstOrDefault(x => x.PersonID == item.PersonID);
                }

                if (item.ModifiedUserID != 0)
                {
                    item.ModifiedUser = db.Persons.FirstOrDefault(x => x.PersonID == item.ModifiedUserID);
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
