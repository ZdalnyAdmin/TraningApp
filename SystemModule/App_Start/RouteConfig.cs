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
                name: "ReloadRegisterUser",
                url: "Templates/registerUser",
                defaults: new { controller = "Redirect", action = "RegisterUser", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Register",
                url: "registerUser",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DeleteOrganization",
                url: "deleteOrganization",
                defaults: new { controller = "Redirect", action = "DeleteOrganization", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DelOrganization",
                url: "delOrganization",
                defaults: new { controller = "Account", action = "DeleteOrganization", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ReloadDeleteOrganization",
                url: "Templates/delOrganization",
                defaults: new { controller = "Redirect", action = "DeleteOrganization", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChangeOrganizationName",
                url: "changeOrganizationName",
                defaults: new { controller = "Redirect", action = "ChangeOrganizationName", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChangeOrganization",
                url: "changeOrganization",
                defaults: new { controller = "Account", action = "ChangeOrganizationName", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ReloadChangeOrganization",
                url: "Templates/changeOrganization",
                defaults: new { controller = "Redirect", action = "ChangeOrganizationName", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "File",
                url: "File/{guid}/{fileName}",
                defaults: new { controller = "File", action = "Index", guid = UrlParameter.Optional, fileName = UrlParameter.Optional }
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
