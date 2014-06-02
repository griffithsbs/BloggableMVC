using Com.GriffithsBen.BloggableMVC.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    public class Element : IElement {

        private string ProxyName { get; set; }

        private string HtmlName { get; set; }

        private string OpenProxyTag {
            // TODO
            get {
                return string.Format("[{0}]", this.ProxyName);
            }
        }

        private string CloseProxyTag {
            // TODO
            get {
                return string.Format("[/{0}]", this.ProxyName);
            }
        }

        private string RawContext { get; set; }

        private List<IElement> Children { get; set; }

        // TODO protected List<ElementAttribute> Attributes { get; set; }

        protected string GetProxyTagWithAttributes() {
            return "TODO";
        }

        protected Element() { }

        public Element(string context, string proxyName) {
            this.RawContext = context;
            this.Children = new List<IElement>();
            // TODO this.Attributes = new List<ElementAttribute>();
            this.ProxyName = proxyName;
            this.HtmlName = MarkupConfiguration.GetHtmlNameFor(proxyName);
            this.Interpret(context);
        }

        private void AddChild(IElement child) {
            this.Children.Add(child);
        }

        //TODO public void AddAttribute(ElementAttribute attribute) {
        //    this.Attributes.Add(attribute);
        //}

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.OpenProxyTag);
            foreach (Element child in this.Children) {
                builder.Append(child.ToString());
            }
            builder.Append(this.CloseProxyTag);
            return builder.ToString();
        }

        public virtual MvcHtmlString GetHtml() {
            TagBuilder tagBuilder = new TagBuilder(this.HtmlName);
            // TODO
            //foreach(ElementAttribute attribute in this.Attributes) {
            //    tagBuilder.MergeAttribute(attribute.HtmlName, attribute.Value);
            //}
            StringBuilder content = new StringBuilder();
            foreach (IElement child in this.Children) {
                content.Append(child.GetHtml());
            }
            tagBuilder.InnerHtml = content.ToString();
            return new MvcHtmlString(tagBuilder.ToString());
        }

        public virtual int GetTextLength() {
            return this.Children.Sum(x => x.GetTextLength());
        }

        // TODO
        bool IElement.IsValid() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">the context that remains to be interpreted</param>
        /// <param name="result">the result so far</param>
        /// <returns></returns>
        internal void Interpret(string context) {

            // if the context is an empty string, interpretation has finished, so return
            if (context.Length == 0) {
                return;
            }

            // try to find an opening proxy tag in the context string

            Regex regex = MarkupConfiguration.OpeningTagsRegex;

            // if no opening proxy tag is found in the context,
            // add a new text node child containing the entire context and return
            if (!regex.IsMatch(context)) {
                IElement textNode = new TextNode(context);
                this.AddChild(textNode);
                return;
            }

            // a match was found

            Match match = regex.Match(context);

            // if an opening tag is found
            // intermediateResult is a MarkupElement

            // if the opening tag was not at position 0,
            // add a text node child to intermediateResult containing the text up to the start of the opening tag
            // and remove that portion of context from the start of the string

            if (match.Index != 0) {
                IElement textNode = new TextNode(context.Substring(0, match.Index));
                this.AddChild(textNode);
                context = context.Substring(match.Index);
            }

            // find the corresponding closing tag and add that portion of the string as a new child of intermediateResult
            // if the closing tag is not the end of the context, make a recursive call to this.Interpret, passing the
            // remainder of the context, having added intermediateResult as a new child of result

            string proxyName = match.Value.Substring(1, match.Value.Length - 2);

            MarkupElement matchedMarkupElement = MarkupConfiguration.GetMarkupElementForMatch(match.Value);

            string endTag = matchedMarkupElement.CloseProxyTag;

            int endOfMatchedElementContent = context.IndexOf(endTag);

            if (endOfMatchedElementContent == -1) {
                // TODO - matching end tag not found - therefore markup is invalid.
                throw new NotImplementedException();
            }

            string startTag = match.Value;
            Element matchedElement = new Element(
                context.Substring(0 + startTag.Length, endOfMatchedElementContent - startTag.Length),
                proxyName
            );

            this.AddChild(matchedElement);

            if (endOfMatchedElementContent < context.Length - endTag.Length) {
                this.Interpret(context.Remove(0, startTag.Length + matchedElement.RawContext.Length + endTag.Length));
            }

            // the closing tag was the end of the context
        }

        IElement IElement.Clone() {
            return new Element(this.RawContext, this.ProxyName);
        }

        IElement IElement.Truncate(int textEndIndex) {
            return (this as IElement).Truncate(textEndIndex, "...");
        }

        IElement IElement.Truncate(int textEndIndex, string textToAppend) {

            if (textEndIndex >= this.GetTextLength()) {
                return (this as IElement).Clone();
            }

            Element result = new Element(string.Empty, this.ProxyName);

            int totalTextLength = 0;

            List<IElement>.Enumerator enumerator = this.Children.GetEnumerator();

            while (enumerator.MoveNext()) {
                IElement child = enumerator.Current;
                int childTextLength = child.GetTextLength();
                if (totalTextLength + childTextLength > textEndIndex) {
                    // cut-off point is inside this node
                    result.AddChild(child.Clone().Truncate(textEndIndex - totalTextLength, textToAppend));
                    return result;
                }
                totalTextLength += childTextLength;
                result.AddChild(child.Clone());
            }

            return result;
        }

    }

}
