using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class RedirectController : Controller
    {
        private string[] _blackList = new[] { "signin", "resetPassword" };

        public ActionResult Index(string id, string trainingID)
        {
            if (_blackList.Contains(id))
            {
                logoff();
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect("/");
            }
            else
            {
                var path = "/?page=" + id;

                if (!string.IsNullOrWhiteSpace(trainingID))
                {
                    path += "&trainingID=" + trainingID;
                }

                return Redirect(path);
            }
        }

        public ActionResult ResetPasswordConfirmation(string code, string id)
        {
            logoff();

            if (string.IsNullOrWhiteSpace(code))
            {
                return Redirect("/");
            }
            else
            {
                code = Url.Encode(code);
                id = Url.Encode(id);
                return Redirect("/?page=resetPasswordConfirmation&code=" + code + "&id=" + id);
            }
        }

        public ActionResult RegisterUser(string code, string id)
        {
            logoff();

            if (string.IsNullOrWhiteSpace(code))
            {
                return Redirect("/");
            }
            else
            {
                code = Url.Encode(code);
                id = Url.Encode(id);
                return Redirect("/?page=registerUser&controller=Templates&code=" + code + "&id=" + id);
            }
        }

        [AllowAnonymous]
        public ActionResult ChangeEmail(string code, string id)
        {
            if (User.Identity.GetUserId() != id)
            {
                logoff();
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return Redirect("/");
            }
            else
            {
                code = Url.Encode(code);
                id = Url.Encode(id);
                return Redirect("/?page=changeUserEmail&controller=Templates&code=" + code + "&id=" + id);
            }
        }

        public ActionResult ActiveTraining(int trainingID)
        {
            return Redirect("/?page=ActiveTraining&trainingID=" + trainingID);
        }

        #region Private Methods
        private void logoff()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
        #endregion
    }
}