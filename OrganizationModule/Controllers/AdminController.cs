using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
using System.Web.Mvc;
using System.Linq;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        #region Private Fields

        private EFContext _db = new EFContext();

        #endregion

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
            Person currentUser = Person.GetLoggedPerson(User);

            if (string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                return new HttpNotFoundResult("Użytkownik nie zalogowany");
            }

            bool allTrainings = true;

            if (currentUser.Profile == ProfileEnum.Administrator)
            {
                allTrainings = false;
                //get protector
                var protector = _db.Users.FirstOrDefault(x => x.Profile == ProfileEnum.Administrator && x.OrganizationID == currentUser.OrganizationID);

                AppSetting setting = null;
                if (protector != null)
                {
                    setting = _db.AppSettings.FirstOrDefault(x => x.ProtectorID == protector.Id);
                }

                allTrainings = setting != null ? setting.IsTrainingAvailableForAll : false;
            }

            ViewBag.User = currentUser;
            ViewBag.showKenpro = allTrainings;

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