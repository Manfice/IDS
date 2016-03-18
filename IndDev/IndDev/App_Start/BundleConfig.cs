using System.Web;
using System.Web.Optimization;

namespace IndDev
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/slide").Include("~/Scripts/slider.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/mairala/css").Include(
                "~/Content/mairala/style.css",
                "~/Content/mairala/slider.css",
                "~/Content/mairala/fwslider.css"
                ));

            bundles.Add(new ScriptBundle("~/mairala/js").Include(
                "~/Scripts/mairala/css3-mediaqueries.js",
                "~/Scripts/mairala/easing.js",
                "~/Scripts/mairala/fwslider.js",
                "~/Scripts/mairala/jquery-ui.min.js",
                "~/Scripts/mairala/jquery.cslider.js",
                "~/Scripts/mairala/jquery.min.js",
                "~/Scripts/mairala/menu.js",
                "~/Scripts/mairala/modernizr.custom.28468.js",
                "~/Scripts/mairala/move-top.js",
                "~/Scripts/mairala/responsive.menu.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/reset.css",      
                "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Main.css",
                      "~/Content/fonts.css",
                      "~/Content/slider.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/media").Include("~/Content/media.css"));
        }
    }
}
