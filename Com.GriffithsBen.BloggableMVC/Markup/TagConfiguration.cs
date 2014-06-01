using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    public class TagConfiguration {

        private static Dictionary<string, string> Tags { get; set; }

        private static List<string> ProxyTags { // TODO these things will need to have some behaviour rather than just being dumb strings
            get {
                return TagConfiguration.Tags.Keys.ToList();
            }
        }

        private static string OpeningTagsRegexFactor {
            get {
                StringBuilder builder = new StringBuilder();

                builder.Append(@"\[(");

                foreach (string proxy in TagConfiguration.ProxyTags) {
                    builder.Append(proxy);
                    if (proxy != TagConfiguration.ProxyTags.Last()) {
                        builder.Append(@"|");
                    }
                }

                builder.Append(@")\]");

                return builder.ToString();
            }
        }

        static TagConfiguration() {
            TagConfiguration.Tags = new Dictionary<string, string>();
            Tags.Add("tag", "div");
            Tags.Add("p", "p");
            Tags.Add("i", "i");
            Tags.Add("quote", "blockquote");
        }

        public static Regex OpeningTagsRegex {
            get {
                return new Regex(TagConfiguration.OpeningTagsRegexFactor);
            }
        }

        public static string GetHtmlNameFor(string proxyName) {
            return TagConfiguration.Tags[proxyName];
        }

    }


}
