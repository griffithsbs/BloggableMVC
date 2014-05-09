using System.Web;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.SampleClient {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
