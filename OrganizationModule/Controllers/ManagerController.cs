using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;

namespace OrganizationModule.Controllers
{
    public class ManagerController : Controller
    {
        #region Private Fields
        private EFContext _db = new EFContext();
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
        public ActionResult EditTranings()
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
    }
}