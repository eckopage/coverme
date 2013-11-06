using System.Web;
using System.Web.Optimization;

namespace musicapp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js",
            "~/Scripts/jquery-ui.unobtrusive-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/FileUpload/css").Include(
                                        "~/Content/FileUpload/tmpl.min.js",
                                        "~/Content/FileUpload/tmpl.min.js",
                                        "~/Content/FileUpload/tmpl.min.js",
                                        "~/Content/FileUpload/canvas-to-blob.min.js",
                                        "~/Content/FileUpload/load-image.min.js",
                                        "~/Content/FileUpload/jquery.iframe-transport.js",
                                        "~/Content/FileUpload/jquery.fileupload.js",
                                        "~/Content/FileUpload/jquery.fileupload-ip.js",
                                        "~/Content/Bootstrap/bootstrap-image-gallery.min.js",
                                        "~/Content/Bootstrap/bootstrap.min.js",
                                        "~/Content/FileUpload/jquery.fileupload-ui.js",
                                        "~/Content/FileUpload/locale.js",
                                         "~/Content/FileUpload/main.js"
                                        ));
            bundles.Add(new ScriptBundle("~/Content/slider/").Include("~/Content/slider/jquery.flexslider-min.js"));
        }
    }
}