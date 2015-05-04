using System.Web;
using System.Web.Optimization;

namespace SystemModule
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

            bundles.Add(new ScriptBundle("~/bundles/SystemModuleApp")
                .Include("~/Scripts/App.js"));


            bundles.Add(new ScriptBundle("~/bundles/SystemModuleAppComponents")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/Factories", "*.js")
                .IncludeDirectory("~/Scripts/Services", "*.js")
                .IncludeDirectory("~/Scripts/Directives", "*.js")
                .Include("~/Scripts/SystemModuleApp.js"));

            bundles.Add(new ScriptBundle("~/FroalaScripts")
                .IncludeDirectory("~/thirdParty/FroalaWysiwygEditor/js", "*.js")
                .IncludeDirectory("~/thirdParty/FroalaWysiwygEditor/js/langs", "*.js")
                .IncludeDirectory("~/thirdParty/FroalaWysiwygEditor/js/plugins", "*.js"));

            bundles.Add(new StyleBundle("~/FroalaStyles").IncludeDirectory(
                "~/thirdParty/FroalaWysiwygEditor/css", "*.css"));

        }
    }
}
