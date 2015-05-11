using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers.Api
{
    public class TraningsResultController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TrainingResult> Get()
        {
            //get from correct profile
            //todo add question result
            var result = db.TrainingResults.OrderByDescending(x => x.EndDate).OrderByDescending(x => x.StartDate).ToList();

            foreach (var item in result)
            {
                item.Person = (from t in db.Users
                               where t.Id == item.PersonID
                               select t).FirstOrDefault();

                item.Training = (from t in db.Trainings
                                 where t.TrainingID == item.TrainingID
                                 select t).FirstOrDefault();

                item.Training.Questions = (from t in db.TrainingQuestons
                                           where t.TrainingID == item.TrainingID
                                           select t).ToList();

                item.Training.Questions.ForEach(y =>
                {
                    y.Answers = (from t in db.TrainingAnswers
                                 where t.TrainingQuestionID == y.TrainingQuestionID
                                 select t).ToList();
                });
            }


            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
