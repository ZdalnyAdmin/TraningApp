using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers.Api
{
    public class ManageTrainingController : ApiController
    {
        private EFContext db = new EFContext();

        public HttpResponseMessage Post(Training obj)
        {
            try
            {
                var all = obj.TrainingType == TrainingType.Kenpro;

                var logged = Person.GetLoggedPerson(User);
                

                //get from correct profile
                List<Training> list = (from t in db.Trainings
                                       join to in db.TrainingsInOrganizations on t.TrainingID equals to.TrainingID
                                       where to.OrganizationID == logged.OrganizationID && t.TrainingType == TrainingType.Internal
                                       select t).ToList();

                if (list == null)
                {
                    list = new List<Training>();
                }

                if (all)
                {
                    var temp = (from t in db.Trainings
                                where t.TrainingType == TrainingType.Kenpro
                                select t).ToList();
                    if (temp != null && temp.Any())
                    {
                        list.AddRange(temp);
                    }
                }

                foreach (var item in list)
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == item.CreateUserID);
                    if (user == null)
                    {
                        continue;
                    }
                    item.SetCreateUserName(user.DisplayName);
                    var runCounter = db.TrainingResults.Count(x => x.TrainingID == item.TrainingID);
                    item.SetRunTrainingStats(runCounter);

                    var groups = (from pg in db.TrainingInGroups
                                  join g in db.Groups on pg.ProfileGroupID equals g.ProfileGroupID
                                  where pg.TrainingID == item.TrainingID
                                  select g.Name).ToList();
                    item.SetAssignedGroups(groups);
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, list.OrderBy(x=>x.CreateDate));
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
