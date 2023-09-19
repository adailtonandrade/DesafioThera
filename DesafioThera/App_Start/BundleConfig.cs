using System.Collections.Generic;
using System.Web.Optimization;

namespace DesafioThera
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/MaterialDesignValidate.js"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/toastr/js").Include(
                        "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/toastr/css").Include(
            "~/Content/toastr.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            #region MultiSelect
            bundles.Add(new StyleBundle("~/bundles/multiselect/css").NonOrdering().Include(
                "~/Metronic/assets/global/plugins/bootstrap-select/css/bootstrap-select.css",
                "~/Metronic/assets/global/plugins/jquery-multi-select/css/multi-select.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/multiselectJs").NonOrdering().Include(
                "~/Metronic/assets/global/plugins/bootstrap-select/js/bootstrap-select.js",
                "~/Metronic/assets/global/plugins/jquery-multi-select/js/jquery.multi-select.js"
                ));
            #endregion
        }
    }
    class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }

    static class BundleExtentions
    {
        public static Bundle NonOrdering(this Bundle bundle)
        {
            bundle.Orderer = new NonOrderingBundleOrderer();
            return bundle;
        }
    }
}
