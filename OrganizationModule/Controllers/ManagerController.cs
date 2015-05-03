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
    [Authorize]
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
            Person currentUser = Person.GetLoggedPerson(User);

            if (string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                return new HttpNotFoundResult("Użytkownik nie zalogowany");
            }

            ViewBag.User = currentUser;

            var invitedUsers = _db.Users.Where(user => user.OrganizationID == currentUser.OrganizationID).OrderByDescending(x=> x.InvitationDate).ToList();
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
                    Person currentUser = Person.GetLoggedPerson(User);
                    var jsonResult = new Result() { Succeeded = false, Errors = new List<string>() };

                    if (string.IsNullOrWhiteSpace(currentUser.Id))
                    {
                        jsonResult.Errors.Add("Użytkownik nie jest zalogowany");
                        return Json(jsonResult);
                    }

                    var previousInvitedUsers = _db.Users.Where(x => x.Email.Equals(model.Email) && x.OrganizationID ==currentUser.OrganizationID).ToList();
                    if (previousInvitedUsers != null && 
                        previousInvitedUsers.Exists(x => x.Status != StatusEnum.Rejected && 
                        x.Status != StatusEnum.Reinvited &&
                        x.Status != StatusEnum.Invited))
                    {
                        jsonResult.Errors.Add("Użytkownik o podanym mailu został już dodany do organizacji");
                        return Json(jsonResult);
                    }
                    else
                    {
                        previousInvitedUsers.ForEach(x =>
                        {
                            if (x.Status == StatusEnum.Invited)
                            {
                                x.Status = StatusEnum.Reinvited;
                            }
                        });

                        await _db.SaveChangesAsync();
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

        [HttpPost]
        public async Task<JsonResult> RemoveInvitation(RemoveInvitationModel model)
        {
            if(ModelState.IsValid)
            {
                var user = UserManager.FindById(model.Id);
                if(user == null)
                {
                    var errors = new List<string>();
                    errors.Add("Podany użytkownik nie istnieje");

                    return Json(new Result() { Succeeded = false, Errors = errors }); 
                }

                user.Status = StatusEnum.Rejected;

                var result = await UserManager.UpdateSecurityStampAsync(user.Id);

                if (!result.Succeeded)
                {
                    return Json(new Result() { Succeeded = result.Succeeded, Errors = new List<string>(result.Errors) });
                }

                result = await UserManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return Json(new Result() { Succeeded = result.Succeeded, Errors = new List<string>(result.Errors) });
                }

                return Json(new Result() { Succeeded = true });
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