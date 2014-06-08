using Com.GriffithsBen.BloggableMVC.Abstract;
using Com.GriffithsBen.BloggableMVC.Configuration;
using Com.GriffithsBen.BloggableMVC.Markup;
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
        public IElement Markup { get; private set; }

        public IEnumerable<MarkupElement> MarkupElements { get; private set; }

        public Markupable(IMarkupable markupableContent) {
            this.MarkupableContent = markupableContent;
            this.MarkupElements = MarkupConfiguration.CopyMarkupElements();
            this.Markup = new Element(MarkupConfiguration.RootElementTagContext, markupableContent.Content);
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

        public MvcHtmlString ContentHtml {
            get {
                return this.Markup.GetHtml();
            }
        }

        public MvcHtmlString TruncateContentHtml(int length) {
            return this.Markup.Truncate(length, MarkupConfiguration.SynopsisEnd).GetHtml();
        }

    }

}
