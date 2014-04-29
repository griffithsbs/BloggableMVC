using System.Web;
using System.Web.Mvc;

namespace Com.GriffithsBen.BlogEngine.SampleClient {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
