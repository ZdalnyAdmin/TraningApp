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
using AppEngine.Models.ViewModels.Account;
using System.Collections.ObjectModel;
using AppEngine.Services;
using System.Collections.Generic;

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

            ViewBag.AdminEmails = _db.Users.Where(x => x.Profile == ProfileEnum.Administrator && x.OrganizationID == currentUser.OrganizationID)
                                           .Select(x => x.Email)
                                           .ToList();

            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> DeleteUser(string code, string id)
        {
            code = code.Replace(' ', '+');
            var usereDeleted = false;
            var user = UserManager.FindById(id);
            var isTokenValid = await UserManager.UserTokenProvider.ValidateAsync("DELETE_USER", code, UserManager, user);

            if (isTokenValid && (DateTime.Now - user.DeleteUserDate.Value).Days < 3)
            {
                var deletedPerson = UserManager.FindById(id);
                deletedPerson.IsDeleted = true;
                deletedPerson.DeletedDate = DateTime.Now;
                await UserManager.UpdateAsync(deletedPerson);

                var organization = _db.Organizations.FirstOrDefault(x => x.OrganizationID == user.OrganizationID);

                var deleteUser = UserManager.FindById(deletedPerson.DeleteUserID);

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                LogService.InsertUserLogs(deletedPerson.DeleteUserID == deletedPerson.Id ? OperationLog.UserDeleteBySelf : OperationLog.UserDelete, _db, deletedPerson.Id, deletedPerson.DeleteUserID, 
                    organization != null ? organization.OrganizationID : 0);

                if (deletedPerson.Id == user.Id)
                {
                    UserManager.SendEmail(organization.ProtectorID,
                           "Usunięcie Użytkownika",
                           "W dniu " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "Użytkownik o Id " + deletedPerson.Id
                           + "i nazwie wyświetlanej: " + deletedPerson.DisplayName
                           + " usunął swoje konto z organizacji " + (organization != null ? organization.Name : "Brak nazwy organizacji"));
                }
                else
                {
                    UserManager.SendEmail(organization.ProtectorID,
                           "Usunięcie Użytkownika",
                           "W dniu " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "Użytkownik o Id " + deletedPerson.Id
                           + "i nazwie wyświetlanej: " + deletedPerson.DisplayName
                           + " usunął swoje konto z organizacji " + (organization != null ? organization.Name : "Brak nazwy organizacji")
                           + " przez użytkownika o Id " + deleteUser.Id + "oraz nazwie wyświetlanej: " + deleteUser.DisplayName);
                }

                usereDeleted = true;
            }
            else
            {
                usereDeleted = false;
            }

            ViewBag.UserDeleted = usereDeleted;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUser()
        {
            var loggedUser = UserManager.FindById(User.Identity.GetUserId());
            loggedUser.DeleteUserID = loggedUser.Id;
            await UserManager.UpdateAsync(loggedUser);

            //loggedUser.Organization = _db.Organizations.FirstOrDefault(x => x.OrganizationID == loggedUser.OrganizationID);

            var result = await loggedUser.DeleteUserAsync(UserManager, Request);

            return Json(result);
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
            var trainingResults = _db.TrainingResults.Where(x => x.PersonID == loggedPerson.Id && x.EndDate.HasValue).OrderByDescending(x=>x.EndDate).ToList();
            trainingResults.ForEach(x =>
                                        {
                                            x.Training = _db.Trainings.FirstOrDefault(y => y.TrainingID == x.TrainingID);
                                            x.Training.UserName = _db.Users.FirstOrDefault(y => y.Id == x.Training.CreateUserID).DisplayName;
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

        [HttpPost]
        public async Task<JsonResult> ChangePassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var loggedUser = Person.GetLoggedPerson(User);

                if (model.UserName == loggedUser.UserName)
                {
                    Result result = new Result();
                    model.Code = await UserManager.GeneratePasswordResetTokenAsync(loggedUser.Id);

                    result = await Person.ChangePasswordAsync(UserManager, model);
                    if (result.Succeeded)
                    {
                        result.Message = "Hasło zostało zmienione!";
                    }

                    return Json(result);
                }
                else
                {
                    loggedUser = UserManager.FindById(loggedUser.Id);
                   var result = await loggedUser.ResetPasswordAsync(UserManager, Request);

                   return Json(result);
                }
            }

            return getErrorsFromModel();
        }

        [HttpPost]
        public async Task<JsonResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = Person.GetLoggedPerson(User);
                loggedUser = UserManager.FindById(loggedUser.Id);

                if(!loggedUser.ChangeEmailDate.HasValue || loggedUser.ChangeEmailDate.Value.Date < DateTime.Now.Date)
                {
                    loggedUser.ChangeEmailDate = DateTime.Now;
                    loggedUser.DailyChangeMailCount = 0;
                } 
                else if(loggedUser.DailyChangeMailCount > 2)
                {
                    var jsonResult = new Result() { Errors = new System.Collections.Generic.List<string>(), Succeeded = false };
                    jsonResult.Errors.Add("Wykorzystałeś maksymalną dzienną liczbę zmian.");
                    return Json(jsonResult);
                }

                loggedUser.DailyChangeMailCount = (loggedUser.DailyChangeMailCount ?? 0) + 1;
                var result = await loggedUser.ChangeEmailAsync(UserManager, Request, model.Email);

                return Json(result);
            }

            return getErrorsFromModel();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ChangeEmail(string code, string id)
        {
            var emailChanged = false;
            var user = UserManager.FindById(id);
            var isTokenValid = await UserManager.UserTokenProvider.ValidateAsync("CHANGE_EMAIL", code, UserManager, user);

            if (isTokenValid && (DateTime.Now - user.ChangeEmailDate.Value).Days < 3)
            {
                await UserManager.UpdateSecurityStampAsync(id);
                var oldEmail = user.Email;

                user.Email = user.NewEmail;
                user.NewEmail = string.Empty;
                user.ChangeEmailDate = null;

                UserManager.Update(user);

                var organization = _db.Organizations.FirstOrDefault(x=> x.OrganizationID == user.OrganizationID);

                emailChanged = true;

                await UserManager.SendEmailAsync(user.InviterID, "Zmiana adresu email przez podopiecznego",
                    string.Format("Użytkownik organizacji {0} o Id {1} i mailu {2} (stary email) zmienił swój adres email na nowy {3}", organization.Name, user.Id, oldEmail, user.Email));
            }
            else
            {
                emailChanged = false;
            }

            ViewBag.EmailChanged = emailChanged;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ChangeUserName(ChangeUserNameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new Result() { Errors = new System.Collections.Generic.List<string>() };
                var loggedUser = Person.GetLoggedPerson(User, _db);
                var oldDisplayName = loggedUser.DisplayName;
                loggedUser.DisplayName = model.UserName;
                _db.SaveChanges();

                result.Succeeded = true;

                await UserManager.SendEmailAsync(loggedUser.InviterID, "Zmiana nazwy wyświetlania przez podopiecznego",
                    string.Format("Użytkownik organizacji {0} o Id {1} i nazwie wyświetlania {2} (stara nazwa) zmienił swoją nazwę na {3}", loggedUser.Organization.Name, loggedUser.Id, oldDisplayName, loggedUser.DisplayName));

                return Json(result);
            }

            return getErrorsFromModel();
        }

        #region Private Functions
        private JsonResult getErrorsFromModel()
        {
            var Errors = new Collection<string>();

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Errors.Add(error.ErrorMessage);
                }
            }

            return Json(new
            {
                Succeeded = false,
                Errors = Errors
            });
        }
        #endregion
    }
}