using AppEngine;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        #region Identity
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }
        #endregion

        #region Private Fields
        private EFContext _db = new EFContext();
        #endregion

        /// <summary>
        /// Navigate to logged user view
        /// </summary>
        /// <returns></returns>
        public ActionResult LoggedUser()
        {
            Person currentUser = Person.GetLoggedPerson(User);

            if (currentUser.OrganizationID == null)
            {
                ViewBag.OrganizationName = "Użytkownik nie jest przypisany do organizacji.";
            } 
            else
            {
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

        [HttpPost]
        public void DeleteUser()
        {
            var loggedPerson = Person.GetLoggedPerson(User, _db);
            loggedPerson.DeleteUserID = loggedPerson.Id;
            loggedPerson.IsDeleted = true;
            loggedPerson.DeletedDate = DateTime.Now;
            _db.SaveChanges();

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            UserManager.SendEmail(loggedPerson.InviterID,
                       "Usunięcie Użytkownika",
                       "W dniu " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "Użytkownik o Id " + loggedPerson.Id
                       + "i nazwie wyświetlanej: " + loggedPerson.DisplayName
                       + " usunął swoje konto z organizacji " + loggedPerson.Organization != null ? loggedPerson.Organization.Name : "Brak nazwy organizacji");
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