using AppEngine.Models.Common;
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
            var stats = new Statistic();
            var loggedUser = Person.GetLoggedPerson(User);

            var currentOrganization = db.Organizations.FirstOrDefault(x => x.OrganizationID == loggedUser.OrganizationID);

            if (currentOrganization == null)
            {
                return stats;
            }

            //get from correct profil
            var users = db.Users.Where(x => x.OrganizationID == currentOrganization.OrganizationID).ToList();

            if (users == null)
            {
                return stats;
            }

            stats.ActivePeople = users.Count(x => x.Status == StatusEnum.Active && !x.IsDeleted);
            stats.BlockedPeople = users.Count(x => x.Status == StatusEnum.Deleted  && !x.IsDeleted);
            stats.DeleteAccount = users.Count(x => x.IsDeleted );

            var trainings = (from t in db.TrainingsInOrganizations
                            where t.OrganizationID == currentOrganization.OrganizationID
                            select t.TrainingID).ToList();

            if (trainings != null && trainings.Any())
            {

                stats.StartedTrainings = db.TrainingResults.Count(x => x.StartDate.HasValue && trainings.Contains(x.TrainingID));
                stats.CompletedTrainings = db.TrainingResults.Count(x => x.EndDate.HasValue && trainings.Contains(x.TrainingID));
            }
            var currentDate = DateTime.Now.Date;
            var endDate = DateTime.Now.Date.AddDays(7);

            stats.WeekActiveUser = users.Count(x => x.LastActivationDate >= currentDate && x.LastActivationDate <= endDate);

            endDate = DateTime.Now.Date.AddDays(30);
            stats.MonthActiveUser = users.Count(x => x.LastActivationDate >= currentDate && x.LastActivationDate <= endDate);
            return stats;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
