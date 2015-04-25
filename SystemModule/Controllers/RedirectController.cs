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
        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect("/");
            }
            else
            {
                return Redirect("/?page=" + id);
            }
        }
    }
}