﻿using AppEngine;
using AppEngine.Models.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SystemModule.Controllers
{
    [Authorize]
    public class MainController : Controller
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

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Navigate to logged user view
        /// </summary>
        /// <returns></returns>
        public ActionResult LoggedUser()
        {
            ViewBag.loggedUser = Person.GetLoggedPerson(User);
            return View();
        }

        public ActionResult CreateOrganization()
        {
            return View();
        }

        public ActionResult OrganizationsList()
        {
            return View();
        }

        public ActionResult GlobalSetting()
        {
            return View();
        }

        public ActionResult Statistics()
        {
            return View();
        }

        public ActionResult CreateTraning()
        {
            return View();
        }

        public ActionResult Tranings()
        {
            return View();
        }

        public ActionResult CreateProtectorRole()
        {
            return View();
        }

        public ActionResult EditProtectorRole()
        {
            return View();
        }

        public ActionResult InternalTrainings()
        {
            return View();
        }

        public ActionResult GlobalAdmins()
        {
            return View();
        }

        public ActionResult History()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ResetUserPassword()
        {
            var loggedUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var result = await loggedUser.ResetPasswordAsync(UserManager, Request);

            return Json(result);
        }
    }
}