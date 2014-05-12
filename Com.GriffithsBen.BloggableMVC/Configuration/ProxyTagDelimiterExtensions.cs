using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GriffithsBen.BloggableMVC.Configuration {
    internal static class ProxyTagDelimiterExtensions {

        internal static char GetOpeningCharacter(this ProxyTagDelimiter delimiter) {
            switch (delimiter) {
                case ProxyTagDelimiter.SquareBracket:
                    return '[';
                case ProxyTagDelimiter.RoundedBracket:
                    return '(';
                case ProxyTagDelimiter.BackTick:
                    return '`';
                case ProxyTagDelimiter.Pipe:
                    return '|';
                case ProxyTagDelimiter.Hash:
                    return '#';
                case ProxyTagDelimiter.AtSign:
                    return '@';
                case ProxyTagDelimiter.Equals:
                    return '=';
                case ProxyTagDelimiter.Power:
                    return '^';
                case ProxyTagDelimiter.Tilde:
                    return '~';
                default:
                    throw new NotSupportedException(delimiter.ToString());
            }
        }

        internal static char GetClosingCharacter(this ProxyTagDelimiter delimiter) {
            switch (delimiter) {
                case ProxyTagDelimiter.SquareBracket:
                    return ']';
                case ProxyTagDelimiter.RoundedBracket:
                    return ')';
                default:
                    return delimiter.GetOpeningCharacter();
            }
        }

        internal static string GetOpenTagFormat(this ProxyTagDelimiter delimiter) {
            return delimiter.GetOpeningCharacter() + "{0}" + delimiter.GetClosingCharacter();
        }

        internal static string GetCloseTagFormat(this ProxyTagDelimiter delimiter) {
            return delimiter.GetOpeningCharacter() + "/{0}" + delimiter.GetClosingCharacter();
        }
    }
}
