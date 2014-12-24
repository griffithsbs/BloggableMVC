using Com.GriffithsBen.BloggableMVC.Markup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.GriffithsBen.BloggableMVC.Configuration {

    /// <summary>
    /// Provides application-wide definitions and configuration of markup elements and attributes
    /// </summary>
    public static class MarkupConfiguration {

        // TODO allow full configuration (add/remove/clear) of elements and of valid and mandatory attributes

        private const string DefaultRootElementTagContext = "[p]";

        private const string DefaultSynopsisEnd = "...";

        private static List<MarkupAttribute> mUniversalAttributes;

        private static List<MarkupAttribute> UniversalAttributes {
            get {
                if (mUniversalAttributes == null) {
                    mUniversalAttributes = new List<MarkupAttribute>() {
                        new MarkupAttribute("class")
                    };
                }
                return mUniversalAttributes;
            }
        }

        private static Dictionary<string, List<MarkupAttribute>> mAttributesForElements;

        private static Dictionary<string, List<MarkupAttribute>> AttributesForElements {
            get {
                if (mAttributesForElements == null) {
                    mAttributesForElements = new Dictionary<string, List<MarkupAttribute>>();
                        mAttributesForElements.Add("link", 
                                                    new List<MarkupAttribute>() { 
                                                        new MarkupAttribute("url", "href"),
                                                        new MarkupAttribute("title")
                                                    }
                        );
                }
                return mAttributesForElements;
            }
        }

        private static Dictionary<string, List<ElementAttribute>> mMandatoryAttributesForElements;

        private static Dictionary<string, List<ElementAttribute>> MandatoryAttributesForElements {
            get {
                if (mMandatoryAttributesForElements == null) {
                    mMandatoryAttributesForElements = new Dictionary<string, List<ElementAttribute>>();
                    mMandatoryAttributesForElements.Add("b",
                                                new List<ElementAttribute>() { 
                                                    new ElementAttribute("class", "bold")
                                                }
                    );
                }
                return mMandatoryAttributesForElements;
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

        /// <summary>
        /// The list of mark up elements that are used by default when parsing marked up content
        /// </summary>
        private static List<MarkupElement> DefaultMarkupElements = new List<MarkupElement>() {
            new MarkupElement("b", "span"),
            new MarkupElement("i", "i"),
            new MarkupElement("p", "p"),
            new MarkupElement("quote", "blockquote"),
            new MarkupElement("link", "a"),
            new MarkupElement("code", "pre")
        };

        /// <summary>
        /// The static collection of markup elements that will be used to initialise the MarkUpElements collection
        /// of all new blog entries.
        /// On initialisation, this is equal to MarkupConfiguration.DefaultMarkupElements
        /// </summary>
        private static List<MarkupElement> MarkupElements { get; set; }

        /// <summary>
        /// Static initialiser
        /// </summary>
        static MarkupConfiguration() {
            MarkupConfiguration.MarkupElements = MarkupConfiguration.DefaultMarkupElements;
            ProxyTagDelimiter proxyTagDelimiter;
            Enum.TryParse(ConfigurationManager.AppSettings["BloggableMVC.MarkupConfiguration.ProxyTagDelimiter"] ?? "SquareBracket",
                          out proxyTagDelimiter);
            MarkupConfiguration.ProxyTagDelimiter = proxyTagDelimiter;

            MarkupConfiguration.RootElementTagContext =
                ConfigurationManager.AppSettings["BloggableMVC.MarkupConfiguration.RootElementTagContext"]
                ?? MarkupConfiguration.DefaultRootElementTagContext;

            MarkupConfiguration.SynopsisEnd =
                ConfigurationManager.AppSettings["BloggableMVC.MarkupConfiguration.SynopsisEnd"]
                ?? MarkupConfiguration.DefaultSynopsisEnd;

        }

        public static Regex OpeningTagsRegex {
            get {
                return new Regex(MarkupConfiguration.OpeningTagsRegexFactor);
            }
        }

        public static string GetHtmlNameForElement(string proxyName) {
            MarkupElement tag = MarkupConfiguration.MarkupElements.Where(x => x.ProxyElement == proxyName).SingleOrDefault();
            if (tag == null) {
                throw new ArgumentException(string.Format("No tag found with proxy name {0}", proxyName));
            }
            return tag.HtmlElement;
        }

        public static string GetHtmlNameForAttribute(string proxyName) {

            if (UniversalAttributes != null) {
                var attrs = UniversalAttributes.Where(x => x.ProxyName == proxyName);
                if(attrs.Any()) {
                    if(attrs.Count() > 1) {
                        throw new ArgumentException(string.Format(
                            "More than one defined universal attribute for the proxy name \"{0}\"", proxyName));
                    }
                    return attrs.Single().HtmlName;
                }
            }

            var specificAttrs = AttributesForElements.SelectMany(x => x.Value)
                                                     .Where(y => y.ProxyName == proxyName);

            if (!specificAttrs.Any()) {
                throw new ArgumentException(string.Format("No attribute found for proxy name {0}", proxyName));
            }

            if (specificAttrs.Count() > 1) {
                throw new ArgumentException(string.Format(
                            "More than one defined attribute for the proxy name \"{0}\"", proxyName));
            }

            return specificAttrs.Single()
                                .HtmlName;
        }
        
        public static string RootElementTagContext { get; private set; }

        /// <summary>
        /// The string to append to the end of the truncated content of a text node
        /// </summary>
        public static string SynopsisEnd { get; set; }

        private static int DefaultSynopsisLength = 20;

        public static int SynopsisLength {
            get {
                int result;
                string config = ConfigurationManager.AppSettings["BloggableMVC.MarkupConfiguration.SynopsisLength"];

                if (Int32.TryParse(config, out result)) {
                    return result;
                }
                return MarkupConfiguration.DefaultSynopsisLength;
            }
        }

        public static List<MarkupAttribute> GetValidAttributesForElement(string elementProxyName) {
            List<MarkupAttribute> attributes = null;
            MarkupConfiguration.AttributesForElements.TryGetValue(elementProxyName, out attributes);
            return attributes ?? new List<MarkupAttribute>();
        }

        public static IEnumerable<ElementAttribute> GetMandatoryAttributesForElement(string elementProxyName) {
            List<ElementAttribute> attributes = null;
            MarkupConfiguration.MandatoryAttributesForElements.TryGetValue(elementProxyName, out attributes);
            return attributes ?? new List<ElementAttribute>();
        }

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

        /// <summary>
        /// Empty the list of currently configured MarkupElements
        /// </summary>
        public static void ClearMarkupElements() {
            MarkupConfiguration.MarkupElements = new List<MarkupElement>();
        }

        public static MarkupElement GetMarkupElementForMatch(string matchValue) {
            IEnumerable<MarkupElement> matchedMarkupElements = 
                MarkupElements.Where(x => new Regex(x.OpenProxyTagRegexFactor).IsMatch(matchValue));

            if (!matchedMarkupElements.Any()) {
                throw new ArgumentException(
                    string.Format("No matching MarkupElement found for matched tag \"{0}\"", matchValue));
            }
            if (matchedMarkupElements.Count() > 1) {
                throw new ArgumentException(
                    string.Format("More than one matching MarkupElement found for matched tag \"{0}\"", matchValue));
            }
            return matchedMarkupElements.Single();
        }

        public static void AddMandatoryAttribute(string proxyElement, ElementAttribute attribute) {
            if (string.IsNullOrWhiteSpace(proxyElement)) {
                throw new ArgumentException("argument is null or whitespace", "proxyElement");
            }
            if (attribute == null) {
                throw new ArgumentNullException("attribute");
            }
            Dictionary<string, List<ElementAttribute>> mandatoryAttributes = MarkupConfiguration.MandatoryAttributesForElements;
            List<ElementAttribute> mandatoryAttributesForThisElement = new List<ElementAttribute>() {
                attribute
            };
            if (mandatoryAttributes.ContainsKey(proxyElement)) {
                if (mandatoryAttributes[proxyElement] == null) {
                    mandatoryAttributes[proxyElement] = new List<ElementAttribute>() {
                        attribute
                    };
                }
                else {
                    mandatoryAttributes[proxyElement].Add(attribute);
                }
            }
            else {
                mandatoryAttributes.Add(proxyElement, mandatoryAttributesForThisElement);
            }
        }

        // TODO allow removal of mandatory attributes

    }

}
