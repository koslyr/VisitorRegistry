using System.Web;
using System.Web.Optimization;

namespace VisReg.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js",
                       "~/Scripts/jquery.maskedinput.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                       "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                     "~/Content/jquery-ui.css",
                     "~/Content/jquery-ui.min.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/accordion.css",
                        "~/Content/themes/base/all.css",
                        "~/Content/themes/base/autocomplete.css",
                        "~/Content/themes/base/base.css",
                        "~/Content/themes/base/button.css",
                        "~/Content/themes/base/core.css",
                        "~/Content/themes/base/datepicker.css",
                        "~/Content/themes/base/dialog.css",
                        "~/Content/themes/base/draggable.css",
                        "~/Content/themes/base/menu.css",
                        "~/Content/themes/base/progressbar.css",
                        "~/Content/themes/base/resizable.css",
                        "~/Content/themes/base/selectable.css",
                        "~/Content/themes/base/selectmenu.css",
                        "~/Content/themes/base/slider.css",
                        "~/Content/themes/base/sortable.css",
                        "~/Content/themes/base/spinner.css",
                        "~/Content/themes/base/tabs.css",
                        "~/Content/themes/base/theme.css",
                        "~/Content/themes/base/tooltip.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Utils").Include(
                        "~/Scripts/App/Session.js",
                        "~/Scripts/App/Validation-OnTheFly.js"));

            
            //Bundles for TELERIK-KENDO.
            #region BundlesTelerik
            //JS files for TELERIK-KENDO.
            bundles.Add(new ScriptBundle("~/Scripts/kendo/js").Include(
                       "~/Scripts/Kendo/kendo.all.min.js",
                       "~/Scripts/Kendo/kendo.aspnetmvc.min.js",
                       "~/Scripts/Kendo/jszip.min.js"));
            //CSS files for TELERIK-KENDO.
            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
                         "~/Content/kendo/kendo.common.min.css",
                         "~/Content/kendo/kendo.default.min.css"));
            //JS files for TELERIK-KENDO Culture.
            bundles.Add(new ScriptBundle("~/kendo/culture").Include(
                       "~/Scripts/Kendo/Culture/kendo.culture.el.min.js",
                       "~/Scripts/Kendo/Culture/kendo.culture.el-GR.min.js"));
            #endregion

            
            //Bundles for TOASTR.
            #region BundlesToastr
            //JS files for Toastr.
            bundles.Add(new ScriptBundle("~/Scripts/toastr/js").Include(
                       "~/Scripts/Toastr/toastr.min.js"));
            //CSS files for Toastr.
            bundles.Add(new StyleBundle("~/Content/toastr/css").Include(
                        "~/Content/Toastr/toastr.css"));
            #endregion

            bundles.IgnoreList.Clear();


#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
                 BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
