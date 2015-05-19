using AppEngine.Helpers;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        [Access(ProfileEnum.Protector)]
        public ActionResult Roles()
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

        #endregion

        #region Logs

        /// <summary>
        /// Navigate to application logs view
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Protector)]
        public ActionResult Logs()
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

        #endregion Logs
    }
}