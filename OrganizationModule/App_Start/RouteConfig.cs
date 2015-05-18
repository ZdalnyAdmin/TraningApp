using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OrganizationModule
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
                name: "ChangeEmail",
                url: "changeEmail",
                defaults: new { controller = "Redirect", action = "ChangeEmail", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChangeUserEmail",
                url: "changeUserEmail",
                defaults: new { controller = "User", action = "ChangeEmail", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DeleteUser",
                url: "deleteUser",
                defaults: new { controller = "User", action = "DeleteUser", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ReloadChangeUserEmail",
                url: "Templates/changeUserEmail",
                defaults: new { controller = "Redirect", action = "ChangeEmail", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ReloadTraining",
                url: "ActiveTraining/{trainingID}",
                defaults: new { controller = "Redirect", action = "ActiveTraining", trainingID = "1" }
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
                name: "Redirect",
                url: "{id}",
                defaults: new { controller = "Redirect", action = "Index" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional, code = UrlParameter.Optional }
            );

        }
    }
}
