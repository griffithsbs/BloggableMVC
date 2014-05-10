using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GriffithsBen.BloggableMVC.Concrete {
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
        /// Replaces all instances of this MarkupElement's Proxy tags with its corresponding HTML tags and 
        /// returns the result
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

        public string AppendProxyEndTagTo(string target) {
            return string.Format("{0}{1}", target, this.CloseProxyTag);
        }

        /// <summary>
        /// Removes all occurrences of this MarkupElement's OpenProxy and CloseProxy tags from the target string
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

        private bool Encloses(string target, int index, bool inclusive) {
            while (target.Length > 0) {

                int startTagStartIndex = target.IndexOf(this.OpenProxyTag);

                if (startTagStartIndex == -1) {
                    // no start tag found in the remainder of the string
                    return false;
                }

                int startTagEndIndex = startTagStartIndex + this.OpenProxyTag.Length;
                int endTagStartIndex = target.IndexOf(this.CloseProxyTag, startTagEndIndex);

                if (endTagStartIndex == -1) {
                    // no end tag found in the remainder of the string
                    return false;
                }

                int endTagEndIndex = endTagStartIndex + this.OpenProxyTag.Length;

                bool encloses = false;

                if (inclusive) {
                    encloses = index >= startTagStartIndex && index <= endTagEndIndex;
                }
                else {
                    encloses = index > startTagEndIndex && index < endTagStartIndex;
                }

                if (encloses) {
                    return true;
                }

                // check the remainder of the string
                target = target.Substring(endTagEndIndex);
            }

            return false;
        }

        /// <summary>
        /// Returns true if the index within the target string is between the start and end tags of 
        /// an instance of this MarkupElement, including within the start and end tags themselves
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool TagEncloses(string target, int index) {
            return this.Encloses(target, index, true);
        }

        /// <summary>
        /// Returns true if the index within the target string is inside an instance of this MarkupElement
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ElementEncloses(string target, int index) {
            return this.Encloses(target, index, false);
        }
    }
}
