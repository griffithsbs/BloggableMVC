using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GriffithsBen.BloggableMVC.Extensions {
    public static class StringExtensions {

        public static string GetOpeningProxyTag(this string elementName) {
            return string.Format("[{0}]", elementName);
        }

        public static string GetClosingProxyTag(this string elementName) {
            return string.Format("[/{0}]", elementName);
        }

    }
}
