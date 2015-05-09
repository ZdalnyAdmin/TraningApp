using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class TrainingController : Controller
    {
        #region Private fields
        private EFContext _db = new EFContext();
        #endregion

        /// <summary>
        /// Navigate to create traning template view
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTemplate()
        {
            return View();
        }

        /// <summary>
        /// Navigate to created tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult EditTemplate()
        {
            return View();
        }

        /// <summary>
        /// Navigate to description how to create traning
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        public ActionResult ActiveTraining(int id)
        {
            var loggedPerson = Person.GetLoggedPerson(User);

            var org2Train = _db.TrainingsInOrganizations
                               .Where(x=>x.OrganizationID == loggedPerson.OrganizationID)
                               .Select(x=>x.TrainingID)
                               .ToList();

            if(org2Train.IndexOf(id) == -1)
            {
                ViewBag.AccessDenied = true;
            }
            else
            {
                ViewBag.AccessDenied = false;
                var training = _db.Trainings.FirstOrDefault(x => x.TrainingID == id);
                var trainingDetails = _db.TrainingDetails.Where(x => x.TrainingID == training.TrainingID).OrderBy(x => x.DisplayNo).ToList();
                training.SetCreateUserName(_db.Users.FirstOrDefault(x => x.Id == training.CreateUserID).DisplayName);
                var questions = _db.TrainingQuestons.Where(x => x.TrainingID == training.TrainingID).OrderBy(x => x.DisplayNo).ToList();

                var result = _db.TrainingResults.FirstOrDefault(x => x.PersonID == loggedPerson.Id);

                if (result == null)
                {
                    // Temp - should be deleted after adding training list.
                    result = new TrainingResult();
                    result.PersonID = loggedPerson.Id;
                    result.StartDate = DateTime.Now;
                    result.TrainingID = training.TrainingID;
                    _db.TrainingResults.Add(result);
                    _db.SaveChanges();
                    ViewBag.StartDate = result.StartDate;

                    // ViewBag.AccessDenied = true; - this is correct statement result.
                }
                else
                {
                    ViewBag.StartDate = result.StartDate;
                }

                questions.ForEach(x =>
                {
                    x.Answers = _db.TrainingAnswers.Where(y => y.TrainingQuestionID == x.TrainingQuestionID).ToList();
                });

                ViewBag.GenerateDate = DateTime.Now;
                ViewBag.Training = training;
                ViewBag.TrainingDetails = trainingDetails;
                ViewBag.Questions = questions;
            }

            return View();
        }

        [HttpPost]
        public JsonResult CheckDate(CheckTrainingDate model)
        {
            Result result = new Result() { Succeeded = false };

            if(ModelState.IsValid)
            {
                var training = _db.Trainings.FirstOrDefault(x => x.TrainingID == model.TrainingID);

                if (training != null)
                {
                    result.Succeeded = !(training.ModifiedDate < model.GenereateDate);
                }
            }

            return Json(result);
        }
    }
}