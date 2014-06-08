using Com.GriffithsBen.BloggableMVC.Concrete;
using Com.GriffithsBen.BloggableMVC.Markup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.GriffithsBen.BloggableMVC.Configuration {

    /// <summary>
    /// 
    /// </summary>
    public static class MarkupConfiguration {

        private const string DefaultRootElementProxyName = "p";

        private const string DefaultSynopsisEnd = "...";

        private static Dictionary<string, List<MarkupAttribute>> mAttributesForElements;

        private static Dictionary<string, List<MarkupAttribute>> AttributesForElements {
            get {
                if (mAttributesForElements == null) {
                    mAttributesForElements = new Dictionary<string, List<MarkupAttribute>>();
                        mAttributesForElements.Add("link", new List<MarkupAttribute>() { 
                        new MarkupAttribute("url", "href"),
                        new MarkupAttribute("title")
                    });
                }
                return mAttributesForElements;
            }
        }
        private static string OpeningTagsRegexFactor {
            get {
                StringBuilder builder = new StringBuilder();

                builder.Append("(");

                foreach (MarkupElement markupElement in MarkupConfiguration.MarkupElements) {
                    builder.Append(markupElement.OpenProxyTagRegexFactor);
                    if (markupElement != MarkupElements.Last()) {
                        builder.Append(@"|");
                    }
                }

                builder.Append(")");

                return builder.ToString();
            }
        }

        public static Regex OpeningTagsRegex {
            get {
                return new Regex(MarkupConfiguration.OpeningTagsRegexFactor);
            }
        }

        public static string GetHtmlNameFor(string proxyName) {
            MarkupElement tag = MarkupConfiguration.MarkupElements.Where(x => x.ProxyElement == proxyName).SingleOrDefault();
            if (tag == null) {
                throw new ArgumentException(string.Format("No tag found with proxy name {0}", proxyName));
            }
            return tag.HtmlElement;
        }
        
        // TODO should maybe make this a tag rather than just a dumb string?
        public static string RootElementProxyName { get; private set; }

        /// <summary>
        /// The string to append to the end of the truncated content of a text node
        /// </summary>
        public static string SynopsisEnd { get; set; }

        public static List<MarkupAttribute> GetValidAttributesForElement(string elementProxyName) {
            List<MarkupAttribute> attributes = null;
            MarkupConfiguration.AttributesForElements.TryGetValue(elementProxyName, out attributes);
            return attributes ?? new List<MarkupAttribute>();
        }

        // TODO tidy

        static MarkupConfiguration() {
            MarkupConfiguration.MarkupElements = MarkupConfiguration.DefaultMarkupElements;
            MarkupConfiguration.ProxyTagDelimiter = ProxyTagDelimiter.SquareBracket; // TODO make configurable
            
            MarkupConfiguration.RootElementProxyName = 
                ConfigurationManager.AppSettings["BloggableMVC.MarkupConfiguration.RootElementProxyName"]
                ?? MarkupConfiguration.DefaultRootElementProxyName;
            
            MarkupConfiguration.SynopsisEnd =
                ConfigurationManager.AppSettings["BloggableMVC.MarkupConfiguration.SynopsisEnd"]
                ?? MarkupConfiguration.DefaultSynopsisEnd;

        }

        private static List<MarkupElement> DefaultMarkupElements = new List<MarkupElement>() {
            new MarkupElement("b", "span class=\"bold\""),
            new MarkupElement("i", "i"),
            new MarkupElement("p", "p"),
            new MarkupElement("quote", "blockquote"),
            new MarkupElement("link", "a")
        };

        /// <summary>
        /// The static collection of markup elements that will be used to initialise the MarkUpElements collection
        /// of all new blog entries
        /// </summary>
        private static List<MarkupElement> MarkupElements { get; set; }

        public static ProxyTagDelimiter ProxyTagDelimiter { get; set; }

        /// <summary>
        /// CopyMarkupElements
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

        public static MarkupElement GetMarkupElementForMatch(string matchValue) {
            // TODO need to handle more gracefully the exception where more than one element matches
            MarkupElement matchedMarkupElement = MarkupConfiguration.MarkupElements
                                                                    .Where(x => new Regex(x.OpenProxyTagRegexFactor).IsMatch(matchValue))
                                                                    .SingleOrDefault();
            if (matchedMarkupElement == null) {
                throw new ArgumentException(string.Format("No matching MarkupElement found for matched tag \"{0}\"", matchValue));
            }
            return matchedMarkupElement;
        }
    }
}
