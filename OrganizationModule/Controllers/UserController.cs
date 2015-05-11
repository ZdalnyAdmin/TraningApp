using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        #region Private Fields
        private EFContext _db = new EFContext();
        #endregion

        /// <summary>
        /// Navigate to logged user view
        /// </summary>
        /// <returns></returns>
        public ActionResult LoggedUser()
        {
            string currentUserId = User.Identity.GetUserId();
            Person currentUser = _db.Users.FirstOrDefault(x => x.Id == currentUserId);

            if (currentUser == null)
            {
                return Redirect("/");
            }

            if (currentUser.OrganizationID == null)
            {
                ViewBag.OrganizationName = "Użytkownik nie jest przypisany do organizacji.";
            } 
            else
            {
                currentUser.Organization = _db.Organizations.FirstOrDefault(org => org.OrganizationID == currentUser.OrganizationID);
                if (currentUser.Organization == null)
                {
                    ViewBag.OrganizationName = "Organizacja do której należał użytkownik nie istnieje.";
                }
                else
                {
                    ViewBag.Organization = currentUser.Organization;
                }
            }

            ViewBag.AdminEmails = _db.Users.Where(x => x.Profile == ProfileEnum.Administrator)
                                           .Select(x => x.Email)
                                           .ToList();

            return View();
        }

        /// <summary>
        /// Navigate to all tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult TrainingList()
        {
            return View();
        }
        /// <summary>
        /// Navigate to available tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult AvailableTrainingList()
        {
            return View();
        }

        /// <summary>
        /// Navigate to available tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult ActiveTraining()
        {
            return View();
        }

        /// <summary>
        /// Navigate to user traning result view
        /// </summary>
        /// <returns></returns>
        public ActionResult TrainingResult()
        {
            var loggedPerson = Person.GetLoggedPerson(User);
            var trainingResults = _db.TrainingResults.Where(x => x.PersonID == loggedPerson.Id && x.EndDate.HasValue).ToList();
            trainingResults.ForEach(x =>
                                        {
                                            x.Training = _db.Trainings.FirstOrDefault(y => y.TrainingID == x.TrainingID);
                                            x.Training.Questions = _db.TrainingQuestons.Where(y => y.TrainingID == x.TrainingID).ToList();
                                            x.Training.Questions.ForEach(y =>
                                                                            {
                                                                                y.Answers = _db.TrainingAnswers.Where(z => z.TrainingQuestionID == y.TrainingQuestionID).ToList();
                                                                            });
                                        });

            ViewBag.TrainingResults = trainingResults;
            
            return View();
        }

        /// <summary>
        /// Navigate to FAQ view
        /// </summary>
        /// <returns></returns>
        public ActionResult TrainingFaq()
        {
            return View();
        }
    }
}