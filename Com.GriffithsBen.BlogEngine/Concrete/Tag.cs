using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GriffithsBen.BlogEngine.Concrete {
    /// <summary>
    /// A tag to be used in marking up blog content, consisting of a proxy tag to be used by the user
    /// and an HTML tag with which the proxy tag can be replaced in order to display the marked up content
    /// as HTML.
    /// It is assumed that all tags are non-self-closing, i.e. both proxy and HTML tag consist of a pair
    /// of tags to open and close the element.
    /// </summary>
    public class Tag {

        /// <summary>
        /// The name of the tag to be used when marking up blog content by the user
        /// e.g. "b" or "quote"
        /// </summary>
        public string ProxyElement { get; private set; }

        /// <summary>
        /// The name of the HTML tag with which the proxy tag will be replaced
        /// </summary>
        public string HtmlElement { get; private set; }

        public Tag(string proxyElementName, string htmlElementName) {
            this.ProxyElement = proxyElementName;
            this.HtmlElement = htmlElementName;
        }

        /// <summary>
        /// e.g. [b]
        /// </summary>
        private string OpenProxyTag {
            get {
                return string.Format("[{0}]", this.ProxyElement);
            }
        }

        /// <summary>
        /// e.g. [/b]
        /// </summary>
        private string CloseProxyTag {
            get {
                return string.Format("[/{0}]", this.ProxyElement);
            }
        }

        /// <summary>
        /// e.g. <em>
        /// </summary>
        private string OpenHtmlTag {
            get {
                return string.Format("<{0}>", this.HtmlElement);
            }
        }

        /// <summary>
        /// e.g. </em>
        /// </summary>
        private string CloseHtmlTag {
            get {
                return string.Format("</{0}>", this.HtmlElement);
            }
        }

        /// <summary>
        /// Replaces all instances of this Tag's Proxy tags with its corresponding HTML tags and returns the result
        /// </summary>
        /// <param name="target">The string upon which to act</param>
        /// <returns></returns>
        public string ReplaceProxyWithHtml(string target) {
            if (target == null) {
                throw new NullReferenceException("target string is null");
            }
            return target.Replace(this.OpenProxyTag, this.OpenHtmlTag)
                         .Replace(this.CloseProxyTag, this.CloseHtmlTag);
        }

        /// <summary>
        /// Removes all occurrences of this tag's OpenProxy and CloseProxy tags from the target string
        /// and returns the result
        /// </summary>
        /// <param name="target">The string upon which to act</param>
        /// <returns></returns>
        /// 
        // TODO remove?
        public string RemoveProxyTags(string target) {
            if (target == null) {
                throw new NullReferenceException("target string is null");
            }

            return target.Replace(this.OpenProxyTag, string.Empty)
                         .Replace(this.CloseProxyTag, string.Empty);
        }

        public bool TagEncloses(string target, int index) {
            //TODO
            return false;
        }

        public bool ElementEncloses(string target, int index) {
            // TODO
            return false;
        }
    }
}
