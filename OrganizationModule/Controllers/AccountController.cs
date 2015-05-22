using AppEngine;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.ViewModels.Account;
using AppEngine.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class AccountController : Controller
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

        #region Constructor
        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        #endregion

        #region Requests

        #region Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> Login(LoginViewModel model)
        {
            try
            {
                var person = _db.Users.Where(x => x.UserName == model.Email &&
                                                  !x.IsDeleted &&
                                                  x.Status == StatusEnum.Active &&
                                                  x.Profile != ProfileEnum.Superuser).FirstOrDefault();
                if (person == null)
                {
                    return false;
                }

                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return true;
                    default:
                        return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        #endregion

        #region Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = new IdentityResult();
                Result responseResult = new Result() { Errors = new List<string>(), Succeeded = false };

                try
                {
                    var user = UserManager.FindById(model.UserId);
                    var userByName = UserManager.FindByName(model.UserName);
                    bool tokenValidation = await UserManager.UserTokenProvider.ValidateAsync("ResetPassword", model.Token, UserManager, user);

                    if (user == null || (DateTime.Now - user.InvitationDate).Days > 2 || !tokenValidation)
                    {
                        responseResult.Errors.Add("Nieprawidłowy token, lub token wygasł.");
                        return Json(responseResult);
                    }

                    if (userByName != null && user.Id != userByName.Id)
                    {
                        responseResult.Errors.Add("Ten login jest już zajęty. Proszę wybrać inny.");
                        return Json(responseResult);
                    }

                    user.RegistrationDate = DateTime.Now;
                    user.ResetPasswordDate = DateTime.Now;
                    user.Status = StatusEnum.Active;
                    user.UserName = model.UserName;
                    result = UserManager.Update(user);

                    if (!result.Succeeded)
                    {
                        return this.Json(result);
                    }

                    var rslt = await Person.ChangePasswordAsync(UserManager, new ResetPasswordViewModel()
                    {
                        Code = model.Token,
                        Password = model.Password,
                        ConfirmPassword = model.ConfirmPassword,
                        UserName = user.UserName
                    }, false);

                    if (!rslt.Succeeded)
                    {
                        return this.Json(rslt);
                    }

                    LogService.InsertUserLogs(OperationLog.UserRegistration, _db, user.Id, user.Id);

                    await UserManager.UpdateSecurityStampAsync(user.Id);
                    await UserManager.SendEmailAsync(user.Id,
                        "Rejestracja Kenpro",
                        "Zakończyłeś rejestrację. <br/>Twój login to: " + user.UserName
                        + "<br/>Twoja nazwa wyświetlana: " + user.DisplayName
                        + "<br/><a href=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + "/signin\">Zaloguj się</a>");
                }
                catch (Exception ex)
                {

                }

                return this.Json(result);
            }

            return getErrorsFromModel();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Register(string id, string code)
        {
            var user = UserManager.FindById(id);
            ViewBag.User = user;

            if (user != null)
            {
                ViewBag.Organization = _db.Organizations.FirstOrDefault(x => x.OrganizationID == user.OrganizationID);
                ViewBag.isTokenValid = await UserManager.UserTokenProvider.ValidateAsync("ResetPassword", code, UserManager, user);
                ViewBag.tokenExpired = (DateTime.Now - user.InvitationDate).Days > 2 || user.Status == StatusEnum.Rejected;
            }
            else
            {
                ViewBag.isTokenValid = false;
                ViewBag.tokenExpired = true;
            }

            return View();
        }
        #endregion

        #region Reset Password
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userByUserName = await UserManager.FindByNameAsync(model.UserName);
                var userByMail = await UserManager.FindByEmailAsync(model.Email);

                if (userByMail == null || !userByMail.Equals(userByUserName))
                {
                    return Json(new
                    {
                        Succeeded = false,
                        Errors = new string[] { "Nie ma użytkownika o takim adresie email" }
                    });
                }

                var result = await userByUserName.ResetPasswordAsync(UserManager, Request);

                return Json(result);
            }

            return getErrorsFromModel();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> ResetPasswordConfirmation(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                Result result = new Result();

                result = await Person.ChangePasswordAsync(UserManager, model);
                return Json(result);
            }

            return getErrorsFromModel();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation(ForgotPasswordViewModel model)
        {
            return View();
        }
        #endregion

        #region Logoff
        [HttpPost]
        public bool Logoff(LoginViewModel model)
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return true;
        }

        [AllowAnonymous]
        public ActionResult Logoff()
        {
            return View();
        }
        #endregion

        #region Logged User
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetLoggedUser()
        {
            return Json(Person.GetLoggedPerson(User));
        }
        #endregion

        #region Deleted
        [AllowAnonymous]
        public async Task<bool> DeleteUserMail(Person model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = UserManager.FindById(model.Id);
                    await user.DeleteUserAsync(UserManager, Request);
                }
                catch (Exception ex)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        #endregion

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