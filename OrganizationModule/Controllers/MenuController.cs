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
        public ActionResult KeeperMenu()
        {
            return View();
        }
    }
}