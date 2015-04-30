using System.Web;
using System.Web.Optimization;

namespace OrganizationModule
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/ui-bootstrap-0.12.1.min.js",
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/site.css",
                "~/Content/Menu/menu.css"));

            bundles.Add(new ScriptBundle("~/bundles/OrganizationModuleApp")
               .Include("~/Scripts/App.js"));

            bundles.Add(new ScriptBundle("~/bundles/OrganizationModuleAppComponents")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/Factories", "*.js")
                .IncludeDirectory("~/Scripts/Services", "*.js")
                .IncludeDirectory("~/Scripts/Directives", "*.js")
                .Include("~/Scripts/OrganizationModuleApp.js"));

            bundles.Add(new StyleBundle("~/Trainings").Include(
                "~/Content/Trainings/training-list.css",
                "~/Content/Trainings/active-training.css"));

            bundles.Add(new StyleBundle("~/Login").Include(
                "~/Content/Login/login.css"));

            bundles.Add(new StyleBundle("~/Settings").Include(
                "~/Content/Settings/user-settings.css"));
        }
    }
}
