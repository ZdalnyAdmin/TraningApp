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
        public ActionResult TrainingList()
        {
            return View();
        }
        /// <summary>
        /// Navigate to available tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult AvailableTrainingList()
        {
            return View();
        }

        /// <summary>
        /// Navigate to available tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult ActiveTraining()
        {
            return View();
        }

        /// <summary>
        /// Navigate to user traning result view
        /// </summary>
        /// <returns></returns>
        public ActionResult TrainingResult()
        {
            return View();
        }

        /// <summary>
        /// Navigate to FAQ view
        /// </summary>
        /// <returns></returns>
        public ActionResult TrainingFaq()
        {
            return View();
        }
    }
}