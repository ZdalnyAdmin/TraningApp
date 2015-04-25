using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class RedirectController : Controller
    {
        // GET: Redirect
        public ActionResult Login()
        {
            return Redirect("/?page=login");
        }

        public ActionResult userCurrent()
        {
            return Redirect("/?page=userCurrent");
        }

        public ActionResult userTrainings()
        {
            return Redirect("/?page=userTrainings");
        }

        public ActionResult userAvailableTrainings()
        {
            return Redirect("/?page=userAvailableTrainings");
        }

        public ActionResult userResult()
        {
            return Redirect("/?page=userResult");
        }

        public ActionResult userFaq()
        {
            return Redirect("/?page=userFaq");
        }

        public ActionResult managerResult()
        {
            return Redirect("/?page=managerResult");
        }

        public ActionResult managerEdit()
        {
            return Redirect("/?page=managerEdit");
        }

        public ActionResult managerInvitation()
        {
            return Redirect("/?page=managerInvitation");
        }

        public ActionResult creatorTraining()
        {
            return Redirect("/?page=creatorTraining");
        }

        public ActionResult creatorTrainings()
        {
            return Redirect("/?page=creatorTrainings");
        }

        public ActionResult creatorHowTo()
        {
            return Redirect("/?page=creatorHowTo");
        }

        public ActionResult adminUsers()
        {
            return Redirect("/?page=adminUsers");
        }

        public ActionResult adminGroups()
        {
            return Redirect("/?page=adminGroups");
        }

        public ActionResult adminManage()
        {
            return Redirect("/?page=adminManage");
        }

        public ActionResult adminStats()
        {
            return Redirect("/?page=adminStats");
        }

        public ActionResult adminSettings()
        {
            return Redirect("/?page=adminSettings");
        }

        public ActionResult adminHowTo()
        {
            return Redirect("/?page=adminHowTo");
        }

        public ActionResult protectorRoles()
        {
            return Redirect("/?page=protectorRoles");
        }

        public ActionResult protectorLogs()
        {
            return Redirect("/?page=protectorLogs");
        }
    }
}