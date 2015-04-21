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

            //user
            routes.MapRoute(
                name: "currentUser",
                url: "Main/LoggedUser",
                defaults: new { controller = "Main", action = "LoggedUser" }
            );

            routes.MapRoute(
                name: "createOrganization",
                url: "Main/CreateOrganization",
                defaults: new { controller = "Main", action = "CreateOrganization" }
            );

            routes.MapRoute(
                name: "organizationsList",
                url: "Main/OrganizationsList",
                defaults: new { controller = "Main", action = "OrganizationsList" }
            );

            routes.MapRoute(
                name: "globalSetting",
                url: "Main/GlobalSetting",
                defaults: new { controller = "Main", action = "GlobalSetting" }
            );

            routes.MapRoute(
                name: "statistics",
                url: "Main/Statistics",
                defaults: new { controller = "Main", action = "Statistics" }
            );

            routes.MapRoute(
                name: "createTraning",
                url: "Main/CreateTraning",
                defaults: new { controller = "Main", action = "CreateTraning" }
            );

            routes.MapRoute(
                name: "editTraning",
                url: "Main/EditTraning",
                defaults: new { controller = "Main", action = "EditTraning" }
            );

            routes.MapRoute(
                name: "createProtector",
                url: "Main/CreateProtectorRole",
                defaults: new { controller = "Main", action = "CreateProtectorRole" }
            );

            routes.MapRoute(
                name: "editProtector",
                url: "Main/EditProtectorRole",
                defaults: new { controller = "Main", action = "EditProtectorRole" }
            );

            routes.MapRoute(
                name: "traningsList",
                url: "Main/TraningsList",
                defaults: new { controller = "Main", action = "TraningsList" }
            );

            routes.MapRoute(
                name: "globalAdmins",
                url: "Main/GlobalAdmins",
                defaults: new { controller = "Main", action = "GlobalAdmins" }
            );

            routes.MapRoute(
                name: "history",
                url: "Main/History",
                defaults: new { controller = "Main", action = "History" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
