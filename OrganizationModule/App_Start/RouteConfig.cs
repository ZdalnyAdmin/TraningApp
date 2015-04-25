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
                name: "Login",
                url: "Login",
                defaults: new { controller = "Redirect", action = "Login"}
            );

            //user
            routes.MapRoute(
                name: "userCurrent",
                url: "userCurrent",
                defaults: new { controller = "Redirect", action = "userCurrent" }
            );

            routes.MapRoute(
                name: "userTrainings",
                url: "userTrainings",
                defaults: new { controller = "Redirect", action = "userTrainings" }
            );

            routes.MapRoute(
                name: "userAvailableTrainings",
                url: "userAvailableTrainings",
                defaults: new { controller = "Redirect", action = "userAvailableTrainings" }
            );

            routes.MapRoute(
                name: "userResult",
                url: "userResult",
                defaults: new { controller = "Redirect", action = "userResult" }
            );

            routes.MapRoute(
                name: "userFaq",
                url: "userFaq",
                defaults: new { controller = "Redirect", action = "userFaq" }
            );

            //manager
            routes.MapRoute(
                name: "managerResult",
                url: "managerResult",
                defaults: new { controller = "Redirect", action = "managerResult" }
            );

            routes.MapRoute(
                name: "managerEdit",
                url: "managerEdit",
                defaults: new { controller = "Redirect", action = "managerEdit" }
            );

            routes.MapRoute(
                name: "managerInvitation",
                url: "managerInvitation",
                defaults: new { controller = "Redirect", action = "managerInvitation" }
            );

            //creator
            routes.MapRoute(
               name: "creatorTraining",
               url: "creatorTraining",
               defaults: new { controller = "Redirect", action = "creatorTraining" }
           );

            routes.MapRoute(
                name: "creatorTrainings",
                url: "creatorTrainings",
                defaults: new { controller = "Redirect", action = "creatorTrainings" }
            );

            routes.MapRoute(
                name: "creatorHowTo",
                url: "creatorHowTo",
                defaults: new { controller = "Redirect", action = "creatorHowTo" }
            );

            //admin
            routes.MapRoute(
               name: "adminUsers",
               url: "adminUsers",
               defaults: new { controller = "Redirect", action = "adminUsers" }
            );

            routes.MapRoute(
                name: "adminGroups",
                url: "adminGroups",
                defaults: new { controller = "Redirect", action = "adminGroups" }
            );

            routes.MapRoute(
                name: "adminManage",
                url: "adminManage",
                defaults: new { controller = "Redirect", action = "adminManage" }
            );

            routes.MapRoute(
                name: "adminStats",
                url: "adminStats",
                defaults: new { controller = "Redirect", action = "adminStats" }
            );

            routes.MapRoute(
               name: "adminSettings",
               url: "adminSettings",
               defaults: new { controller = "Redirect", action = "adminSettings" }
           );

            routes.MapRoute(
                name: "adminHowTo",
                url: "adminHowTo",
                defaults: new { controller = "Redirect", action = "adminHowTo" }
            );

            //protector
            routes.MapRoute(
               name: "protectorRoles",
               url: "protectorRoles",
               defaults: new { controller = "Redirect", action = "protectorRoles" }
           );

            routes.MapRoute(
                name: "protectorLogs",
                url: "protectorLogs",
                defaults: new { controller = "Redirect", action = "protectorLogs" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
