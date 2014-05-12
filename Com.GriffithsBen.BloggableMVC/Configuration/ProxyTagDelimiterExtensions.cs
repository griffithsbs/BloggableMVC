using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GriffithsBen.BloggableMVC.Configuration {
    internal static class ProxyTagDelimiterExtensions {

        internal static string GetOpenTagFormat(this ProxyTagDelimiter delimiter) {

            switch (delimiter) {
                case ProxyTagDelimiter.SquareBracket :
                    return "[{0}]";
                case ProxyTagDelimiter.RoundedBracket :
                    return "({0})";
                case ProxyTagDelimiter.BackTick :
                    return "`{0}`";
                case ProxyTagDelimiter.Pipe :
                    return "|{0}|";
                case ProxyTagDelimiter.Hash :
                    return "#{0}#";
                case ProxyTagDelimiter.AtSign :
                    return "@{0}@";
                case ProxyTagDelimiter.Equals :
                    return "={0}=";
                case ProxyTagDelimiter.Power :
                    return "^{0}^";
                case ProxyTagDelimiter.Tilde :
                    return "~{0}~";
                default :
                    throw new NotSupportedException(delimiter.ToString());
            }
        }

        internal static string GetCloseTagFormat(this ProxyTagDelimiter delimiter) {
            return delimiter.GetOpenTagFormat().Replace("{0}", @"/{0}");
        }
    }
}
