using Com.GriffithsBen.BloggableMVC.Configuration;
using System;

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

            if (target == null) {
                throw new NullReferenceException("target");
            }
            if (index < 0) {
                throw new ArgumentException("index cannot be less than zero");
            }
            if (index >= target.Length) {
                throw new ArgumentException("index must be less than target length");
            }

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
                    encloses = index >= startTagEndIndex && index < endTagStartIndex;
                }

                if (encloses) {
                    return true;
                }

                if (target.Length <= (endTagEndIndex + 1)) {
                    return false;
                }

                // check the remainder of the string
                target = target.Substring(endTagEndIndex + 1);
                // reduce the index in proportion to the change in length of the target
                index -= (endTagEndIndex + 1);
                
            }

            return false;
        }

        private bool TagEncloses(string target, int index, string tag) {
            if (target == null) {
                throw new NullReferenceException("target");
            }
            if (index < 0) {
                throw new ArgumentException("index cannot be less than zero");
            }
            if (index >= target.Length) {
                throw new ArgumentException("index must be less than target length");
            }

            while (target.Length > 0) {

                int startIndex = target.IndexOf(tag);

                if (startIndex == -1) {
                    // no tag found in the remainder of the string
                    return false;
                }

                int endIndex = startIndex + tag.Length;
                
                if (index >= startIndex && index < endIndex) {
                    return true;
                }

                if (target.Length <= (endIndex + 1)) {
                    return false;
                }

                // check the remainder of the string
                target = target.Substring(endIndex + 1);
                // reduce the index in proportion to the change in length of the target
                index -= (endIndex + 1);

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
        internal bool Encloses(string target, int index) {
            return this.Encloses(target, index, true);
        }

        /// <summary>
        /// Returns true if the index within the target string is inside an instance of this MarkupElement
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal bool ElementEncloses(string target, int index) {
            return this.Encloses(target, index, false);
        }

        internal bool StartTagEncloses(string target, int index) {
            return this.TagEncloses(target, index, this.OpenProxyTag);
        }

        internal bool EndTagEncloses(string target, int index) {
            return this.TagEncloses(target, index, this.CloseProxyTag);
        }
    }
}
