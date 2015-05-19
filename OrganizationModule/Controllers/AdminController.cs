using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using AppEngine.Helpers;
using System.Reflection;

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
        [Access(ProfileEnum.Administrator)]
        public ActionResult Users()
        {
            Person currentUser = Person.GetLoggedPerson(User);
            if (string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                return new HttpNotFoundResult("Użytkownik nie zalogowany");
            }

            if (!Helpers.CheckAccess(MethodBase.GetCurrentMethod(), currentUser.Profile))
            {
                return new HttpNotFoundResult("1"); // Jedynka to chwilowo brak uprawnień do oglądania strony
            }

            return View();
        }

        #endregion Users lists

        #region Groups

        /// <summary>
        /// Navigate to user group managment view
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Administrator)]
        public ActionResult Groups()
        {
            Person currentUser = Person.GetLoggedPerson(User);
            if (string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                return new HttpNotFoundResult("Użytkownik nie zalogowany");
            }

            if (!Helpers.CheckAccess(MethodBase.GetCurrentMethod(), currentUser.Profile))
            {
                return new HttpNotFoundResult("1"); // Jedynka to chwilowo brak uprawnień do oglądania strony
            }

            return View();
        }

        #endregion Groups

        #region Training managments

        /// <summary>
        /// Navigate to traning managment view
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Administrator)]
        public ActionResult Managment()
        {
            Person currentUser = Person.GetLoggedPerson(User);
            if (string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                return new HttpNotFoundResult("Użytkownik nie zalogowany");
            }

            if (!Helpers.CheckAccess(MethodBase.GetCurrentMethod(), currentUser.Profile))
            {
                return new HttpNotFoundResult("1"); // Jedynka to chwilowo brak uprawnień do oglądania strony
            }

            return View();
        }

        #endregion Training managments

        #region Statistics

        /// <summary>
        /// Navigate to statistics view
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Administrator)]
        public ActionResult Statistics()
        {
            Person currentUser = Person.GetLoggedPerson(User);
            if (string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                return new HttpNotFoundResult("Użytkownik nie zalogowany");
            }

            if (!Helpers.CheckAccess(MethodBase.GetCurrentMethod(), currentUser.Profile))
            {
                return new HttpNotFoundResult("1"); // Jedynka to chwilowo brak uprawnień do oglądania strony
            }

            return View();
        }

        #endregion Statistics

        #region Settings

        /// <summary>
        /// Navigate to global organization settings
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Administrator)]
        public ActionResult Settings()
        {
            Person currentUser = Person.GetLoggedPerson(User);
            if (string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                return new HttpNotFoundResult("Użytkownik nie zalogowany");
            }

            if (!Helpers.CheckAccess(MethodBase.GetCurrentMethod(), currentUser.Profile))
            {
                return new HttpNotFoundResult("1"); // Jedynka to chwilowo brak uprawnień do oglądania strony
            }

            return View();
        }

        #endregion Settings

        /// <summary>
        /// Navigate to about - how to use view
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Administrator)]
        public ActionResult About()
        {
            Person currentUser = Person.GetLoggedPerson(User);
            if (string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                return new HttpNotFoundResult("Użytkownik nie zalogowany");
            }

            if (!Helpers.CheckAccess(MethodBase.GetCurrentMethod(), currentUser.Profile))
            {
                return new HttpNotFoundResult("1"); // Jedynka to chwilowo brak uprawnień do oglądania strony
            }

            return View();
        }
    }
}