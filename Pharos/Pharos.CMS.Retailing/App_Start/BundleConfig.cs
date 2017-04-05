﻿using System.Globalization;
using System.Web;
using System.Web.Optimization;

namespace Pharos.CMS.Retailing
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));
            bundles.Add(
                new ScriptBundle("~/bundles/jquery.validate")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.additional-methods.js")
                .Include("~/Scripts/jquery.validate.custom-methods.js")
                );

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
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

            RegisterScriptBundles(bundles);
            RegisterStyleBundles(bundles);
            #region localization script bundles

            bundles.Add(new StyleBundle("~/bundles/jquery-culture").Include(
                string.Format("~/Scripts/lang/jquery-validation/messages_{0}.js", CultureInfo.CurrentUICulture.TwoLetterISOLanguageName ?? CultureInfo.CurrentUICulture.Name)));

            #endregion
            BundleTable.EnableOptimizations = true;
        }
        public static void RegisterScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                "~/Scripts/easyui-validatebox-ext.js",
                "~/Scripts/easyui-ex.js",
                "~/Scripts/easyui-lang-zh_CN.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/pharos").Include(
                "~/Scripts/jquery-ex.js",
                "~/Scripts/pharos.js"));
            bundles.Add(new ScriptBundle("~/bundles/grid").Include("~/Scripts/comm-grid.js"));
            bundles.Add(new ScriptBundle("~/bundles/form").Include("~/Scripts/comm-form.js"));
        }
        public static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/reset").Include("~/Content/reset.css"));
            bundles.Add(new StyleBundle("~/Content/mythemes/default/pharos").Include(
                "~/Content/mythemes/default/pharos.css"
                ));
            bundles.Add(new StyleBundle("~/Content/themes/bootstrap/easyui").Include("~/Content/themes/bootstrap/easyui.css", "~/Content/themes/bootstrap/customeasyui.css"));
            bundles.Add(new StyleBundle("~/Content/themes/easyui").Include(
                "~/Content/themes/icon.css",
                "~/Content/themes/color.css",
                "~/Content/themes/customcolor.css"
                ));
        }
    }
}