using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class ManagerController : Controller
    {
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
            return View();
        }
    }
}