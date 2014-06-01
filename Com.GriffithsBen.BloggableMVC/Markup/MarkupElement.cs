using Com.GriffithsBen.BloggableMVC.Configuration;
using System;

namespace Com.GriffithsBen.BloggableMVC.Markup {
    /// <summary>
    /// An element to be used in marking up blog content, consisting of a proxy element to be used by the user
    /// and an HTML element with which the proxy element can be replaced in order to display the marked up content
    /// as HTML.
    /// It is assumed that all element are non-self-closing, i.e. both proxy and HTML element consist of a pair
    /// of tags to open and close the element.
    /// </summary>
    public class MarkupElement {

        /// <summary>
        /// The name of the element to be used when marking up blog content by the user
        /// e.g. "b" or "quote"
        /// </summary>
        public string ProxyElement { get; private set; }

        /// <summary>
        /// The name of the HTML element with which the proxy element will be replaced
        /// </summary>
        public string HtmlElement { get; private set; }

        public MarkupElement(string proxyElementName, string htmlElementName) {
            this.ProxyElement = proxyElementName;
            this.HtmlElement = htmlElementName;
            this.ProxyTagDelimiter = MarkupConfiguration.ProxyTagDelimiter;
        }

        private string OpenProxyTagFormat {
            get {
                return this.ProxyTagDelimiter.GetOpenTagFormat();
            }
        }
         
        private string CloseProxyTagFormat {
            get {
                return this.ProxyTagDelimiter.GetCloseTagFormat();
            }
        }

        public ProxyTagDelimiter ProxyTagDelimiter { get; set; }

        /// <summary>
        /// See ProxyTagDelimiterExtensions class for supported tag formats
        /// </summary>
        public string OpenProxyTag {
            get {
                return string.Format(this.OpenProxyTagFormat, this.ProxyElement);
            }
        }

        /// <summary>
        /// See ProxyTagDelimiterExtensions class for supported tag formats
        /// </summary>
        public string CloseProxyTag {
            get {
                return string.Format(this.CloseProxyTagFormat, this.ProxyElement);
            }
        }

    }
}
