using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class StatisticsController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Statistic> Get()
        {
            return null;
        }

        public HttpResponseMessage Post(Statistic stats)
        {
            try
            {
                List<Statistic> statistics = null;
                switch (stats.Type)
                {
                    case StatisticEnum.Global:

                        statistics = (from t in db.Trainings
                                      where !t.IsDeleted
                                      select new Statistic
                                      {
                                          Name = t.Name,
                                          OrganizationNo = db.TrainingsInOrganizations.Count(x => x.Training != null && x.TrainingID == t.TrainingID),
                                          StartedTrainings = db.TrainingResults.Count(x => x.TrainingID == t.TrainingID && x.StartDate.HasValue),
                                          CompletedTrainings = db.TrainingResults.Count(x => x.TrainingID == t.TrainingID && x.EndDate.HasValue),
                                          CreateDate = t.CreateDate
                                      }).ToList();
                        break;
                    case StatisticEnum.Organization:

                        statistics = (from t in db.Organizations
                                      where !t.IsDeleted
                                      select new Statistic
                                      {
                                          Name = t.Name,
                                          PeopleNo = db.Users.Count(x => x.OrganizationID == t.OrganizationID),
                                          InternalTrainings = db.TrainingsInOrganizations.Count(x => x.Training != null && !x.Training.IsDeleted && x.Training.TrainingType == TrainingType.Internal),
                                          CompletedTrainings = (from to in db.TrainingsInOrganizations
                                                                join tr in db.TrainingResults
                                                                on to.TrainingID equals tr.TrainingID
                                                                where to.OrganizationID == t.OrganizationID && tr.EndDate.HasValue
                                                                select tr).Count()
                                      }).ToList();
                        break;
                    case StatisticEnum.General:
                        statistics = new List<Statistic>();
                        var obj = new Statistic();
                        obj.PeopleNo = db.Users.Count(x => x.OrganizationID != null && !x.IsDeleted);
                        obj.OrganizationNo = db.Trainings.Count(x => !x.IsDeleted);
                        obj.InternalTrainings = db.Trainings.Count(x => !x.IsDeleted && x.TrainingType == TrainingType.Internal);
                        obj.StartedTrainings = (from t in db.TrainingResults
                                                where t.StartDate.HasValue
                                                group t by t.TrainingID
                                                    into grp
                                                    select grp).Count();
                        statistics.Add(obj);
                        break;
                    default:
                        break;
                }


                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, statistics);
                return response;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }
    }
}
