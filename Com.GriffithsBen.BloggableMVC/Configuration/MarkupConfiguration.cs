using Com.GriffithsBen.BloggableMVC.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.GriffithsBen.BloggableMVC.Configuration {

    /// <summary>
    /// 
    /// </summary>
    public static class MarkupConfiguration {

        // new from Markup.TagConfiguration:

        private static Dictionary<string, string> Tags { get; set; }

        private static List<string> ProxyTags { // TODO these things will need to have some behaviour rather than just being dumb strings
            get {
                return MarkupConfiguration.Tags.Keys.ToList();
            }
        }

        private static string OpeningTagsRegexFactor {
            get {
                StringBuilder builder = new StringBuilder();

                builder.Append(@"\[(");

                foreach (string proxy in MarkupConfiguration.ProxyTags) {
                    builder.Append(proxy);
                    if (proxy != MarkupConfiguration.ProxyTags.Last()) {
                        builder.Append(@"|");
                    }
                }

                builder.Append(@")\]");

                return builder.ToString();
            }
        }

        public static Regex OpeningTagsRegex {
            get {
                return new Regex(MarkupConfiguration.OpeningTagsRegexFactor);
            }
        }

        public static string GetHtmlNameFor(string proxyName) {
            return MarkupConfiguration.Tags[proxyName];
        }

        public static string RootElementProxyName {
            get {
                return "p";
            }
        }

        // pre-existing - TODO tidy


        static MarkupConfiguration() {
            MarkupConfiguration.MarkupElements = MarkupConfiguration.DefaultMarkupElements;
            MarkupConfiguration.ProxyTagDelimiter = ProxyTagDelimiter.SquareBracket;

            // TODO
            MarkupConfiguration.Tags = new Dictionary<string, string>();
            Tags.Add("tag", "div");
            Tags.Add("p", "p");
            Tags.Add("i", "i");
            Tags.Add("quote", "blockquote");

        }

        private static List<MarkupElement> DefaultMarkupElements = new List<MarkupElement>() {
            new MarkupElement("b", "em"),
            new MarkupElement("i", "i"),
            new MarkupElement("p", "p"),
            new MarkupElement("quote", "blockquote")
        };

        /// <summary>
        /// The static collection of markup elements that will be used to initialise the MarkUpElements collection
        /// of all new blog entries
        /// </summary>
        private static List<MarkupElement> MarkupElements { get; set; }

        public static ProxyTagDelimiter ProxyTagDelimiter { get; set; }
        /// </summary>
        /// <returns>a new collection of markup elements, cloned from MarkupElementConfiguration.MarkupElements</returns>
        public static IEnumerable<MarkupElement> CopyMarkupElements() {
            if (MarkupConfiguration.MarkupElements == null) {
                throw new InvalidOperationException("MarkupElements member is null");
            }
            MarkupElement[] copy = new MarkupElement[MarkupConfiguration.MarkupElements.Count];

            if(copy.Length > 0) {
                MarkupConfiguration.MarkupElements.ToArray().CopyTo(copy, 0);
            }
            return copy;
        }

        public static IEnumerable<MarkupElement> GetMarkupElements() {
            return MarkupConfiguration.MarkupElements;
        }

        /// <summary>
        /// Adds a new element to the MarkupElements collection
        /// </summary>
        /// <param name="proxyElementName"></param>
        /// <param name="htmlElementName"></param>
        public static void AddMarkupElement(string proxyElementName, string htmlElementName) {

            if (proxyElementName == null) {
                throw new ArgumentException("proxyElementName is null");
            }
            if (htmlElementName == null) {
                throw new ArgumentException("htmlElementName is null");
            }
            if (string.IsNullOrWhiteSpace(proxyElementName)) {
                throw new ArgumentException("proxyElementName is white space");
            }
            if (string.IsNullOrWhiteSpace(htmlElementName)) {
                throw new ArgumentException("htmlElementName is white space");
            }

            MarkupConfiguration.MarkupElements.Add(new MarkupElement(proxyElementName, htmlElementName));
        }

        /// <summary>
        /// Removes any MarkupElement instances with the given ProxyElement name from the MarkupElements collection
        /// </summary>
        /// <param name="proxyElementName">the name of the ProxyElement of the element to be removed</param>
        /// <returns></returns>
        public static void RemoveMarkupElement(string proxyElementName) {

            if (proxyElementName == null) {
                throw new ArgumentException("proxyElementName is null");
            }
            
            MarkupElement targetElement = MarkupConfiguration.MarkupElements.Find(x => x.ProxyElement == proxyElementName);
            MarkupConfiguration.MarkupElements.Remove(targetElement);
        }

    }
}
