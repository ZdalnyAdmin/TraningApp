using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        #region Private Fields

        private EFContext _db = new EFContext();

        #endregion

        #region Users list

        /// <summary>
        /// Navigate to user managment view
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            return View();
        }

        #endregion Users lists

        #region Groups

        /// <summary>
        /// Navigate to user group managment view
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups()
        {
            return View();
        }

        #endregion Groups

        #region Training managments

        /// <summary>
        /// Navigate to traning managment view
        /// </summary>
        /// <returns></returns>
        public ActionResult Managment()
        {
            return View();
        }

        #endregion Training managments

        #region Statistics

        /// <summary>
        /// Navigate to statistics view
        /// </summary>
        /// <returns></returns>
        public ActionResult Statistics()
        {
            return View();
        }

        #endregion Statistics

        #region Settings

        /// <summary>
        /// Navigate to global organization settings
        /// </summary>
        /// <returns></returns>
        public ActionResult Settings()
        {
            return View();
        }

        #endregion Settings

        /// <summary>
        /// Navigate to about - how to use view
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }
    }
}