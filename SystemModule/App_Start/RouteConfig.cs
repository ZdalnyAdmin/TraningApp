using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SystemModule
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ResetPassword",
                url: "resetPasswordConfirmation",
                defaults: new { controller = "Redirect", action = "ResetPasswordConfirmation", code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "RegisterUser",
                url: "register",
                defaults: new { controller = "Redirect", action = "RegisterUser", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Redirect",
                url: "{id}",
                defaults: new { controller = "Redirect", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
