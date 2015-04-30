using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using AppEngine.Models.ViewModels.Manager;
using System.Threading.Tasks;
using AppEngine;

namespace OrganizationModule.Controllers
{
    public class ManagerController : Controller
    {
        #region Private Fields
        private EFContext _db = new EFContext();
        #endregion

        //#region Identity
        //private ApplicationUserManager _userManager;
        //private ApplicationSignInManager _signInManager;

        //public ApplicationSignInManager SignInManager
        //{
        //    get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
        //    private set { _signInManager = value; }
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        //    private set { _userManager = value; }
        //}
        //#endregion

        /// <summary>
        /// Navigate to tranings results view
        /// </summary>
        /// <returns></returns>
        public ActionResult Results()
        {
            return View();
        }

        /// <summary>
        /// Navigate to trainig modifications view
        /// </summary>
        /// <returns></returns>
        public ActionResult EditTrainings()
        {
            return View();
        }

        /// <summary>
        /// Navigate to invitation view
        /// </summary>
        /// <returns></returns>
        public ActionResult Invitation()
        {
            string currentUserId = User.Identity.GetUserId();

            if (currentUserId == null)
            {
                return Redirect("/");
            }

            Person currentUser = _db.Users.FirstOrDefault(x => x.Id == currentUserId);

            if (currentUser == null)
            {
                return Redirect("/");
            }

            ViewBag.User = currentUser;

            var invitedUsers = _db.Users.Where(user => user.OrganizationID == currentUser.OrganizationID).ToList();
            ViewBag.InvitedUsers = invitedUsers;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Invitation(InviteUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = new Person { UserName = model.UserName, Email = model.Email };
                //var result = await UserManager.CreateAsync(user, "test");

                //if (!result.Succeeded)
                //{
                //    return this.Json(result);
                //}
                //else
                //{
                //    await UserManager.SendEmailAsync(user.Id,
                //       "Rejestracja Kenpro",
                //       "Zakończyłeś rejestrację. <br/>Twój login to: " + user.UserName
                //       + "<br/>Twoja nazwa wyświetlana: " + user.UserName
                //       + "<br/><a href=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + "/login\">Zaloguj się</a>");
                //}

                //return this.Json(result);
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