using OrganizationModule.Models;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        /// <summary>
        /// Navigate to user managment view
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            return View();
        }

        /// <summary>
        /// Navigate to user group managment view
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups()
        {
            return View();
        }

        /// <summary>
        /// Navigate to traning managment view
        /// </summary>
        /// <returns></returns>
        public ActionResult Managment()
        {
            return View();
        }
        /// <summary>
        /// Navigate to statistics view
        /// </summary>
        /// <returns></returns>
        public ActionResult Statistics()
        {
            return View();
        }

        /// <summary>
        /// Navigate to global organization settings
        /// </summary>
        /// <returns></returns>
        public ActionResult Settings()
        {
            return View();
        }

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