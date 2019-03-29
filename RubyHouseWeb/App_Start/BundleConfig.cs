using System.Web;
using System.Web.Optimization;

namespace RubyHouseWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Homer style
            bundles.Add(new StyleBundle("~/bundles/homer/css").Include(
                      "~/Content/style.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/site/css").Include(
                      "~/Content/Site.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
                      "~/Scripts/jquery-3.3.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/homer/js").Include(
                      "~/Vendor/metisMenu/dist/metisMenu.min.js",
                      "~/Scripts/homer.js",
                      "~/Scripts/common.js"));

            // Plugin JS
            bundles.Add(new ScriptBundle("~/bundles/plugin/js").Include(
                      "~/Vendor/bootstrap/dist/js/bootstrap.min.js",
                      "~/Vendor/bootstrapdatepicker/bootstrap-datepicker.min.js",
                      "~/Vendor/jquery-validation/jquery.validate.min.js",
                      "~/Vendor/datatables/media/js/jquery.dataTables.min.js",
                      "~/Scripts/locales/bootstrap-datepicker.vi.min.js"
                      ));

            // Plugin CSS
            bundles.Add(new StyleBundle("~/bundles/plugin/css").Include(
                      "~/Vendor/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/plugin/bootstrap-datepicker/bootstrap-datepicker.min.css",
                      "~/Content/plugin/datatables/css/dataTables.bootstrap.min.css",
                      "~/Vendor/toastr/build/toastr.min.css"));

            // Animate.css
            bundles.Add(new StyleBundle("~/bundles/animate/css").Include(
                      "~/Vendor/animate.css/animate.min.css"));

            // Font Awesome icons style
            bundles.Add(new StyleBundle("~/bundles/font-awesome/css").Include(
                      "~/Vendor/fontawesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));
        }
    }
}
