using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers.Api
{
    public class StatisticsController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public Statistic Get()
        {
            //get from correct profil
            var stats = new Statistic();
            stats.ActivePeople = db.Persons.Count(x => x.StatusID == 1 && !x.IsDeleted);
            stats.BlockedPeople = db.Persons.Count(x => x.StatusID == 2 && !x.IsDeleted);
            stats.DeleteAccount = db.Persons.Count(x => x.IsDeleted);

            stats.StartedTrainings = db.TrainingResults.Count(x => x.StartDate.HasValue);
            stats.CompletedTrainings = db.TrainingResults.Count(x => x.EndDate.HasValue);

            var currentDate = DateTime.Now.Date;
            var endDate = DateTime.Now.Date.AddDays(7);

            stats.WeekActiveUser = db.Persons.Count(x => x.LastActivationDate >= currentDate && x.LastActivationDate <= endDate);

            endDate = DateTime.Now.Date.AddDays(30);
            stats.MonthActiveUser = db.Persons.Count(x => x.LastActivationDate >= currentDate && x.LastActivationDate <= endDate);
            return stats;
        }
    }
}
