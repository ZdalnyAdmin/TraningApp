using AppEngine.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        [ChildActionOnly]
        public ActionResult UserMenu()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult CreatorMenu()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult ManagerMenu()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult AdminMenu()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult KeeperMenu()
        {
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.LoggedUser = Person.GetLoggedPerson(User);
            return View();
        }
    }
}