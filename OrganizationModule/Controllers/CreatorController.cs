using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class CreatorController : Controller
    {
        #region Create new training

        /// <summary>
        /// Navigate to create traning template view
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTemplate()
        {
            return View();
        }

        #endregion Create new training

        #region Edit training

        /// <summary>
        /// Navigate to created tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult EditTemplate()
        {
            return View();
        }

        #endregion Edit training

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