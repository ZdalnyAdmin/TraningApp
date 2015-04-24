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

            //user
            routes.MapRoute(
                name: "userCurrent",
                url: "User/LoggedUser",
                defaults: new { controller = "User", action = "LoggedUser" }
            );

            routes.MapRoute(
                name: "userTrainings",
                url: "User/TrainingList",
                defaults: new { controller = "User", action = "TrainingList" }
            );

            routes.MapRoute(
                name: "userAvailableTrainings",
                url: "User/AvailableTrainingList",
                defaults: new { controller = "User", action = "AvailableTrainingList" }
            );

            routes.MapRoute(
                name: "userResult",
                url: "User/TrainingResult",
                defaults: new { controller = "User", action = "TrainingResult" }
            );

            routes.MapRoute(
                name: "userFaq",
                url: "User/TrainingFaq",
                defaults: new { controller = "User", action = "TrainingFaq" }
            );

            //manager
            routes.MapRoute(
                name: "managerResult",
                url: "Manager/Results",
                defaults: new { controller = "Manager", action = "Results" }
            );

            routes.MapRoute(
                name: "managerEdit",
                url: "Manager/EditTrainings",
                defaults: new { controller = "Manager", action = "EditTrainings" }
            );

            routes.MapRoute(
                name: "managerInvitation",
                url: "Manager/Invitation",
                defaults: new { controller = "Manager", action = "Invitation" }
            );

            //creator
            routes.MapRoute(
               name: "creatorTraining",
               url: "Creator/CreateTemplate",
               defaults: new { controller = "Creator", action = "CreateTemplate" }
           );

            routes.MapRoute(
                name: "creatorTrainings",
                url: "Creator/TrainingList",
                defaults: new { controller = "Creator", action = "TrainingList" }
            );

            routes.MapRoute(
                name: "creatorHowTo",
                url: "Creator/About",
                defaults: new { controller = "Creator", action = "About" }
            );

            //admin
            routes.MapRoute(
               name: "adminUsers",
               url: "Admin/Users",
               defaults: new { controller = "Admin", action = "Users" }
            );

            routes.MapRoute(
                name: "adminGroups",
                url: "Admin/Groups",
                defaults: new { controller = "Admin", action = "Groups" }
            );

            routes.MapRoute(
                name: "adminManage",
                url: "Admin/Managment",
                defaults: new { controller = "Admin", action = "Managment" }
            );

            routes.MapRoute(
                name: "adminStats",
                url: "Admin/Statistics",
                defaults: new { controller = "Admin", action = "Statistics" }
            );

            routes.MapRoute(
               name: "adminSettings",
               url: "Admin/Settings",
               defaults: new { controller = "Admin", action = "Settings" }
           );

            routes.MapRoute(
                name: "adminHowTo",
                url: "Admin/About",
                defaults: new { controller = "Admin", action = "About" }
            );

            //protector
            routes.MapRoute(
               name: "protectorRoles",
               url: "Protector/Roles",
               defaults: new { controller = "Protector", action = "Roles" }
           );

            routes.MapRoute(
                name: "protectorLogs",
                url: "Protector/Logs",
                defaults: new { controller = "Protector", action = "Logs" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
