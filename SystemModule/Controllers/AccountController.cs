using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SystemModule.Models;

namespace SystemModule.Controllers
{
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

        #region Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return this.Json(result);
            }
            else
            {
                await UserManager.SendEmailAsync(user.Id,
                   "Rejestracja Kenpro",
                   "Zakończyłeś rejestrację. <br/>Twój login to: " + user.UserName
                   + "<br/>Twoja nazwa wyświetlana: " + user.UserName
                   + "<br/><a href=\"" + Request.Url.Scheme + ":\\\\" + Request.Url.Authority + "/login\">Zaloguj się</a>");
            }

            return this.Json(result);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        #endregion

        #region Reset Password
        [AllowAnonymous]
        public ActionResult ResetPassword()
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
        #endregion
    }
}