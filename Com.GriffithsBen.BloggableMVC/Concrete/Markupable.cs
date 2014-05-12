using Com.GriffithsBen.BloggableMVC.Abstract;
using Com.GriffithsBen.BloggableMVC.Configuration;
using Com.GriffithsBen.BloggableMVC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Concrete {

    public class Markupable {

        /// <summary>
        /// The object wrapped by this Markupable object
        /// </summary>
        private IMarkupable MarkupableContent { get; set; }

        public IEnumerable<MarkupElement> MarkupElements { get; private set; }

        public Markupable(IMarkupable markupableContent) {
            this.MarkupableContent = markupableContent;
            this.MarkupElements = MarkupConfiguration.CopyMarkupElements();
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
            MarkupElement brokenTag = null;

            // also find out which, if any, elements will be 'broken' by truncating the string
            // i.e. does the end of the substring to be taken fall inside an instance of one or more of the elements?
            List<MarkupElement> brokenElements = new List<MarkupElement>();

            if (this.MarkupElements != null) {
                foreach (MarkupElement element in this.MarkupElements) {
                    if (element.TagEncloses(result, length - 1)) {

                        if (element.ElementEncloses(result, length - 1)) {
                            brokenElements.Add(element);
                        }
                        else {
                            brokenTag = element;
                        }
                    }
                }
            }

            result = result.Substring(0, length);

            // if there is a broken tag on the end of the result, fix it
            // TODO it could be a start tag or an end tag that is broken
            // if a start tag, it needs to be removed rather than fixed
            if (brokenTag != null) {
                int startOfBrokenTag = result.LastIndexOf(brokenTag.ProxyTagDelimiter.GetOpeningCharacter());
                result = result.Substring(0, startOfBrokenTag);
                result = brokenTag.AppendProxyEndTagTo(result);
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
