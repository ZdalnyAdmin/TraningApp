using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// Navigate to logged user view
        /// </summary>
        /// <returns></returns>
        public ActionResult LoggedUser()
        {
            return View();
        }

        /// <summary>
        /// Navigate to all tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult TreningList()
        {
            return View();
        }
        /// <summary>
        /// Navigate to available tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult AvailableTraningList()
        {
            return View();
        }

        /// <summary>
        /// Navigate to user traning result view
        /// </summary>
        /// <returns></returns>
        public ActionResult TraningResult()
        {
            return View();
        }

        /// <summary>
        /// Navigate to FAQ view
        /// </summary>
        /// <returns></returns>
        public ActionResult TraningFaq()
        {
            return View();
        }
    }
}