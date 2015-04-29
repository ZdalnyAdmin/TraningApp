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
            stats.ActivePeople = db.Users.Count(x => x.Status == StatusEnum.Active && !x.IsDeleted);
            stats.BlockedPeople = db.Users.Count(x => x.Status == StatusEnum.Deleted && !x.IsDeleted);
            stats.DeleteAccount = db.Users.Count(x => x.IsDeleted);

            stats.StartedTrainings = db.TrainingResults.Count(x => x.StartDate.HasValue);
            stats.CompletedTrainings = db.TrainingResults.Count(x => x.EndDate.HasValue);

            var currentDate = DateTime.Now.Date;
            var endDate = DateTime.Now.Date.AddDays(7);

            stats.WeekActiveUser = db.Users.Count(x => x.LastActivationDate >= currentDate && x.LastActivationDate <= endDate);

            endDate = DateTime.Now.Date.AddDays(30);
            stats.MonthActiveUser = db.Users.Count(x => x.LastActivationDate >= currentDate && x.LastActivationDate <= endDate);
            return stats;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
