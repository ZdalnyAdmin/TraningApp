using AppEngine;
using AppEngine.Models;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppEngine.Services;
using System.Data.Entity;
using System.Collections.Generic;

namespace SystemModule.Controllers
{
    public class AccountController : Controller
    {

        #region Private Fields
        private EFContext _db = new EFContext();
        #endregion

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
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = _db.Users.FirstOrDefault(x => x.UserName == model.Email && x.Profile == ProfileEnum.Superuser);
                    if (user != null)
                    {
                        LogService.AdministrationLogs(SystemLog.LogIn, _db, user.Id);
                    }

                    return true;
                default:
                    return false;
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        #endregion

        #region Operator
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> OperatorRegistry(Person model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = new IdentityResult();

                try
                {
                    var logged = Person.GetLoggedPerson(User);
                    var user = new Person
                    {
                        UserName = Guid.NewGuid().ToString(),
                        DisplayName = model.UserName,
                        Email = model.Email,
                        InviterID = logged != null ? logged.Id : null,
                        Profile = model.Profile,
                        Status = StatusEnum.Invited,
                        OrganizationID = model.OrganizationID,
                        InvitationDate = DateTime.Now
                    };

                    result = await UserManager.CreateAsync(user, "Operator123!");
                    if (!result.Succeeded)
                    {
                        return this.Json(result);
                    }
                    else
                    {
                        var organization = _db.Organizations.FirstOrDefault(x => x.OrganizationID == model.OrganizationID);
                        organization.Protector = user;
                        organization.ProtectorID = user.Id;
                        _db.Entry<Organization>(organization).State = EntityState.Modified;
                        _db.SaveChanges();

                        var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                        UserManager.SendEmailAsync(user.Id,
                            "Zaproszenie Kenpro",
                           "Zakończyłeś zaproszony do organizacji:" + (organization != null ? organization.Name : string.Empty)
                           + "<br/>Zaproszenie zostało wysłane przez: " + (logged != null ? logged.UserName : string.Empty)
                           + "<br/><br/><a href=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + "/register?id=" + user.Id + "&code=" + code + "\">Link</a>");

                        LogService.ProtectorLogs(SystemLog.ProtectorInvitation, _db, organization.Name, user.InviterID);
                    }
                }
                catch (Exception ex)
                {

                }

                return this.Json(result);
            }

            return getErrorsFromModel();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> OperatorConfirmRegistration(Person model)
        {
            try
            {
                await UserManager.SendEmailAsync(model.Id,
                    "POTWIERDZENIE REJESTRACJI",
                           "Zakończyłeś rejestrację. <br/>Twój login to: " + model.UserName
                           + "<br/>Twoja nazwa wyświetlana: " + model.UserName
                           + "<br/><a href=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + "/signin\">Zaloguj się</a>");
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
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
                    });

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

        #region Organization

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> OrganizationCreateMail(Organization model)
        {
            try
            {
                await UserManager.SendEmailAsync(model.CreateUserID,
                    "UTWORZENIE ORGANIZACJI",
                    "W dniu " + DateTime.Now.ToString("dd.MM.yyyy hh.mm") + " utworzyles nowa organizacje " + model.Name);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> OrganizationDeleteMail(Organization model)
        {
            try
            {
                await UserManager.SendEmailAsync(model.CreateUserID,
                    "USUNIECIE ORGANIZACJI POTWIERDZENIE",
                           "Nazwa : " + model.Name + " została zgłoszona do usunięcia."
                           + "<br/>Powód : " + model.DeletedReason
                           + "<br/>Jeśli chcesz ją usunać wcisnij link" 
                           + "<br/><a href=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + "/signin\">LINK</a>");
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> OrganizationNameChangesMail(Organization model)
        {
            try
            {
                await UserManager.SendEmailAsync(model.CreateUserID,
                    "POTWIERDZENIE ZMIANY NAZWY ORGANIZACJI",
                           "Nazwa : " + model.Name + " została zmieniona na " + model.Name
                           + " Jeśli zmiana ma być zapisana i aktywowana naciśnij link."
                           + "<br/><a href=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + "/signin\">LINK</a>");
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Reset Password
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
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
        public async Task<ActionResult> ResetAdminPassword(Person model)
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


                if(result.Succeeded)
                {
                    await UserManager.SendEmailAsync(model.Id,
                        "ZADANIE RESETU HASŁA",
                        "Zostało wysłane zadanie zmiany hasła, aby kontynuować, klinknij link"
                        + "<br/><a href=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + "/signin\">LINK</a>");
               
                }

                return Json(result);
            }

            return getErrorsFromModel();
        }

        [HttpPost]
        public async Task<ActionResult> ResetPasswordConfirmation(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                Result result = new Result();

                result = await Person.ChangePasswordAsync(UserManager, model);
                return Json(result);
            }

            return getErrorsFromModel();
        }

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

            var user = Person.GetLoggedPerson(User);
            if (user != null)
            {
                LogService.AdministrationLogs(SystemLog.LogOut, _db, user.Id);
            }

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