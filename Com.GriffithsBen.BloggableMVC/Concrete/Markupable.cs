using Com.GriffithsBen.BloggableMVC.Abstract;
using Com.GriffithsBen.BloggableMVC.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Concrete {

    public class Markupable {

        /// <summary>
        /// The object wrapped by this Markupable object
        /// </summary>
        private IMarkupable MarkupableContent { get; set; }

        // the parsed markup
        public MarkupElement Markup { get; private set; }

        public IEnumerable<MarkupElement> MarkupElements { get; private set; }

        public Markupable(IMarkupable markupableContent) {
            this.MarkupableContent = markupableContent;
            this.MarkupElements = MarkupConfiguration.CopyMarkupElements();
            this.Markup = new MarkupElement(markupableContent.Content, MarkupConfiguration.RootElementProxyName);
        }

        /// <summary>
        /// Adds a new element to the MarkupElement collection for this blog entry
        /// </summary>
        /// <param name="proxyElementName"></param>
        /// <param name="htmlElementName"></param>
        public void AddMarkupElement(string proxyElementName, string htmlElementName) {
            this.MarkupElements.Concat(new List<MarkupElement>() { new MarkupElement(proxyElementName, htmlElementName) });
        }

        /// <summary>
        /// Removes any MarkupElement instances with the given ProxyElement name from the MarkupElements collection
        /// </summary>
        /// <param name="proxyElementName">the name of the ProxyElement of the element to be removed</param>
        /// <returns></returns>
        public void RemoveMarkupElement(string proxyElementName) {
            this.MarkupElements = this.MarkupElements.Except(
                this.MarkupElements.Where(x => x.ProxyElement == proxyElementName)
            );
        }


        private string EncodedHtmlContent {
            get {
                if (this.MarkupableContent.Content == null) {
                    throw new InvalidOperationException("BlogEntry's Content string is null");
                }

                if (this.MarkupElements == null) {
                    return this.MarkupableContent.Content;
                }

                string result = this.MarkupableContent.Content;
                foreach (MarkupElement element in this.MarkupElements) {
                    result = element.ReplaceProxyWithHtml(result);
                }
                return result;
            }
        }

        public MvcHtmlString ContentHtml {
            get {
                return new MvcHtmlString(this.EncodedHtmlContent);
            }
        }

        private string TruncateContent(int length) {

            if (this.MarkupableContent.Content == null) {
                return this.MarkupableContent.Content;
            }

            string result = this.MarkupableContent.Content;

            if (result.Length <= length) {
                return result;
            }

            // find out which, if any, tag will be 'broken' by truncating the string
            // i.e. does the end of the substring to be taken fall inside an instance of one of the tags?
            MarkupElement brokenStartTag = null;
            MarkupElement brokenEndTag = null;

            // also find out which, if any, elements will be 'broken' by truncating the string
            // i.e. does the end of the substring to be taken fall inside an instance of one or more of the elements?
            List<MarkupElement> brokenElements = new List<MarkupElement>();

            if (this.MarkupElements != null) {
                foreach (MarkupElement element in this.MarkupElements) {

                    if (element.StartTagEncloses(result, length - 1)) {
                        brokenStartTag = element;
                    }
                    else {
                        if (element.EndTagEncloses(result, length - 1)) {
                            brokenEndTag = element;
                        }
                        else {
                            if (element.Encloses(result, length - 1)) {
                                brokenElements.Add(element);
                            }
                        }
                    }
                }
            }

            result = result.Substring(0, length);

            // if there is a broken tag on the end of the result, fix it (if it's an end tag)
            // or remove it (if it's a start tag)
            if (brokenStartTag != null) {
                int startOfBrokenTag = result.LastIndexOf(brokenStartTag.ProxyTagDelimiter.GetOpeningCharacter());
                result = result.Substring(0, startOfBrokenTag);
            }
            else {
                if (brokenEndTag != null) {
                    int startOfBrokenTag = result.LastIndexOf(brokenEndTag.ProxyTagDelimiter.GetOpeningCharacter());
                    result = result.Substring(0, startOfBrokenTag);
                    result = brokenEndTag.AppendProxyEndTagTo(result);
                }
            }

            // add a closing tag for each of the broken elements
            // TODO this isn't necessarily going to result in valid HTML
            // we need to test whether the tags are balanced
            foreach (MarkupElement element in brokenElements) {
                result = string.Format("{0}{1}", result, element.CloseProxyTag);
            }

            return result.Substring(0, length);

        }

        private string TruncateEncodedHtmlContent(int length) {
            
            if (this.MarkupElements == null) {
                return this.TruncateContent(length);
            }

            string result = this.TruncateContent(length);
            foreach (MarkupElement element in this.MarkupElements) {
                result = element.ReplaceProxyWithHtml(result);
            }
            return result;
        }

        public MvcHtmlString TruncateContentHtml(int length) {
            return new MvcHtmlString(string.Format("{0}...", this.TruncateEncodedHtmlContent(length)));
        }

    }

}
