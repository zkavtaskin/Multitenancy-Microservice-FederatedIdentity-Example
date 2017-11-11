using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/mustache.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/display").Include(
                "~/Scripts/app/display.js",
                "~/Scripts/app/timeCountUp.js"));

            bundles.Add(new ScriptBundle("~/bundles/broadcast").Include(
                "~/Scripts/app/broadcast.js"));

            bundles.Add(new ScriptBundle("~/bundles/group-create").Include(
                "~/Scripts/app/group.create.js"));

            bundles.Add(new ScriptBundle("~/bundles/group").Include(
                "~/Scripts/app/group.js"));

            bundles.Add(new ScriptBundle("~/bundles/group-view").Include(
                "~/Scripts/app/group.view.js"));

            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                "~/Scripts/app/layout.js"));

            bundles.Add(new ScriptBundle("~/bundles/tenant").Include(
                "~/Scripts/app/tenantsetup.js"));

            bundles.Add(new ScriptBundle("~/bundles/tenantsettings").Include(
                "~/Scripts/app/tenantsettings.js"));

            bundles.Add(new StyleBundle("~/Content/theme").Include(
                "~/Content/themes/base/Site.css",
                "~/Content/themes/base/pure-drawer.css"));

            //BundleTable.EnableOptimizations = true;
        }
    }
}