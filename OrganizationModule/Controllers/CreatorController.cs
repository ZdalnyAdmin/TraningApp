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
    public class CreatorController : Controller
    {
        #region Create new training

        /// <summary>
        /// Navigate to create traning template view
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Creator)]
        public ActionResult CreateTemplate()
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

        #endregion Create new training

        #region Edit training

        /// <summary>
        /// Navigate to created tranings view
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Manager)]
        public ActionResult EditTemplate()
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

        #endregion Edit training

        /// <summary>
        /// Navigate to description how to create traning
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Creator)]
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

        #region Training list

        /// <summary>
        /// Navigate to create traning template view
        /// </summary>
        /// <returns></returns>
        [Access(ProfileEnum.Creator)]
        public ActionResult TemplateList()
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

        #endregion Training list
    }
}