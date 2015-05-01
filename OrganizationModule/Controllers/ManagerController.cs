using AppEngine;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.ViewModels.Manager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class ManagerController : Controller
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
            Person currentUser = Person.GetLoogedPerson(User);

            if (string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                return new HttpNotFoundResult("Użytkownik nie zalogowany");
            }

            ViewBag.User = currentUser;

            var invitedUsers = _db.Users.Where(user => user.OrganizationID == currentUser.OrganizationID).ToList();
            ViewBag.InvitedUsers = invitedUsers;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Invitation(InviteUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Person currentUser = Person.GetLoogedPerson(User);

                    if (string.IsNullOrWhiteSpace(currentUser.UserName))
                    {
                        return Json(new Result() { Succeeded = false, Errors = new List<string>() });
                    }

                    var user = new Person
                    {
                        UserName = Guid.NewGuid().ToString(),
                        DisplayName = model.UserName,
                        Email = model.Email,
                        InviterID = currentUser.Id,
                        Profile = model.Role,
                        Status = StatusEnum.Invited,
                        OrganizationID = currentUser.OrganizationID,
                        InvitationDate = DateTime.Now
                    };

                    var result = await UserManager.CreateAsync(user, "nieWazneHas!0");

                    if (!result.Succeeded)
                    {
                        return this.Json(result);
                    }
                    else
                    {
                        var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                        await UserManager.SendEmailAsync(user.Id,
                           "Zaproszenie Kenpro",
                           "Zakończyłeś zaproszony do organizacji:" + currentUser.Organization.Name
                           + "<br/>Zaproszenie zostało wysłane przez: " + currentUser.UserName
                           + "<br/><br/><a href=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + "/register?id=" + user.Id + "&code=" + code + "\">Link</a>");
                    }

                    return this.Json(result);
                }
            }
            catch (Exception ex)
            {

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