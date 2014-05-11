using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GriffithsBen.BloggableMVC.Extensions {
    internal static class StringExtensions {

        // TODO refactor these methods out of this class - put them into the MarkupElement class, but
        // mark them internal?

        internal static string GetOpeningProxyTag(this string elementName) {
            return string.Format("[{0}]", elementName);
        }

        internal static string GetClosingProxyTag(this string elementName) {
            return string.Format("[/{0}]", elementName);
        }

    }
}
