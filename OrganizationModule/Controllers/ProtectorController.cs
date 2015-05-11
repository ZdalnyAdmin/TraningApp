using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class ProtectorController : Controller
    {

        #region Roles 

        /// <summary>
        /// Navigate to profile and application roles managment view
        /// </summary>
        /// <returns></returns>
        public ActionResult Roles()
        {
            return View();
        }

        #endregion

        #region Logs

        /// <summary>
        /// Navigate to application logs view
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs()
        {
            return View();
        }

        #endregion Logs
    }
}