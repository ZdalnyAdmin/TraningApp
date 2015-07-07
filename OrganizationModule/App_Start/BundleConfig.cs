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
                "~/Scripts/jquery.js",
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/ui-bootstrap-0.12.1.min.js"));

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
                "~/Content/Trainings/active-training.css",
                "~/Content/Trainings/create-training.css",
                "~/Content/Trainings/manage-trainings.css"));

            bundles.Add(new StyleBundle("~/Results").Include(
                "~/Content/Trainings/user-result.css",
                "~/Content/Trainings/results.css"));

            bundles.Add(new StyleBundle("~/Login").Include(
                "~/Content/Login/login.css"));

            bundles.Add(new StyleBundle("~/Settings").Include(
                "~/Content/Settings/user-settings.css"));

            bundles.Add(new StyleBundle("~/Users").Include(
                "~/Content/Users/users.css",
                "~/Content/Users/invite.css"));

            bundles.Add(new StyleBundle("~/Groups").Include(
                "~/Content/Admin/groups.css"));

            bundles.Add(new StyleBundle("~/Company-settings").Include(
                "~/Content/Admin/company-settings.css"));

            bundles.Add(new ScriptBundle("~/FroalaScripts")
                .IncludeDirectory("~/thirdParty/FroalaWysiwygEditor/js", "*.js")
                .IncludeDirectory("~/thirdParty/FroalaWysiwygEditor/js/langs", "*.js")
                .IncludeDirectory("~/thirdParty/FroalaWysiwygEditor/js/plugins", "*.js"));

            bundles.Add(new StyleBundle("~/FroalaStyles").IncludeDirectory(
                "~/thirdParty/FroalaWysiwygEditor/css", "*.css"));
        }
    }
}
