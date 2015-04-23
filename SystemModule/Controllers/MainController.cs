using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemModule.Controllers
{
    public class MainController : Controller
    {
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

        public ActionResult EditTraning()
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

        public ActionResult TraningsList()
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

    }
}