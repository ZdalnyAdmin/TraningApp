using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class CreatorController : Controller
    {
        /// <summary>
        /// Navigate to create traning template view
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTemplate()
        {
            return View();
        }

        /// <summary>
        /// Navigate to created tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult TraningList()
        {
            return View();
        }

        /// <summary>
        /// Navigate to description how to create traning
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }
    }
}