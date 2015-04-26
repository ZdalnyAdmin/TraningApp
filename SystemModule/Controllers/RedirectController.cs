using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class RedirectController : Controller
    {
        public ActionResult Index(string id, string code)
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

        public ActionResult ResetPasswordConfirmation(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Redirect("/");
            }
            else
            {
                code = Url.Encode(code);
                return Redirect("/?page=resetPasswordConfirmation&code=" + code);
            }
        }
    }
}