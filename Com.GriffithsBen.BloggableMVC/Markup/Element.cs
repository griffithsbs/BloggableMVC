using Com.GriffithsBen.BloggableMVC.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    /// <summary>
    /// An instance of a markup element in a parsed content string
    /// </summary>
    public class Element : IElement {

        private string ProxyName { get; set; }

        private string HtmlName { get; set; }

        private string OpenProxyTag { get; set; }

        private string CloseProxyTag {
            get {
                return string.Format(MarkupConfiguration.ProxyTagDelimiter.GetCloseTagFormat(), this.ProxyName);
            }
        }

        private string ContentContext { get; set; }

        private List<IElement> Children { get; set; }

        private List<ElementAttribute> Attributes { get; set; }

        private IMarkupValidator mMarkupValidator { get; set; }

        public Element(string tagContext, string contentContext) {
            this.mMarkupValidator = new MarkupValidator();
            this.OpenProxyTag = tagContext;
            this.ContentContext = contentContext;
            this.Children = new List<IElement>();
            this.Attributes = new List<ElementAttribute>();
            this.ProxyName = this.InterpretTag(tagContext);
            this.HtmlName = MarkupConfiguration.GetHtmlNameForElement(this.ProxyName);
            this.Interpret(contentContext);
        }

        private void AddChild(IElement child) {
            this.Children.Add(child);
        }

        private void AddAttribute(ElementAttribute attribute) {
            this.Attributes.Add(attribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">the context that remains to be interpreted</param>
        /// <param name="result">the result so far</param>
        /// <returns></returns>
        private void Interpret(string context) {

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

            MarkupElement matchedMarkupElement = MarkupConfiguration.GetMarkupElementForMatch(match.Value);

            string endTag = matchedMarkupElement.CloseProxyTag;

            int endOfMatchedElementContent = context.IndexOf(endTag);

            if (endOfMatchedElementContent == -1) {
                // matching end tag not found - markup is invalid.
                this.mMarkupValidator.AddError("Matching end tag not found for element \"{0}\". End tag inserted.", 
                    matchedMarkupElement.ProxyElement);

                // insert a closing tag for the element immediately after the mis-matched opening tag, 
                // and then reinterpret this context
                context = context.Replace(match.Value, string.Format("{0}{1}", match.Value, matchedMarkupElement.CloseProxyTag));
                this.Interpret(context);
                return;
            }

            string startTag = match.Value;
            Element matchedElement = new Element(
                match.Value,
                context.Substring(0 + startTag.Length, endOfMatchedElementContent - startTag.Length)
            );

            this.AddChild(matchedElement);

            if (endOfMatchedElementContent < context.Length - endTag.Length) {
                this.Interpret(context.Remove(0, startTag.Length + matchedElement.ContentContext.Length + endTag.Length));
            }

            // the closing tag was the end of the context
        }

        /// <summary>
        /// Interprets the given tagText in order to set the proxy tag names of this element and
        /// the collection of attributes on this element
        /// </summary>
        /// <param name="tagText"></param>
        /// <returns>the interpreted proxy name of the element as per the given tagText</returns>
        private string InterpretTag(string tagText) {

            MarkupElement matchedElement = MarkupConfiguration.GetMarkupElementForMatch(tagText);

            this.ProxyName = matchedElement.ProxyElement;

            foreach(MarkupAttribute attribute in matchedElement.ValidAttributes) {
                // only the first declaration of an attribute is parsed
                // subsquent duplicate declarations are silently ignored
                int startIndexOfName = tagText.IndexOf(attribute.ProxyName);
                if (startIndexOfName != -1) {

                    int startIndexOfValue = tagText.IndexOf("=\"", startIndexOfName);

                    if (startIndexOfValue != -1) {
                        // advance past the equals sign and opening double-quote
                        startIndexOfValue += 2;
                        int endIndexOfValue = tagText.IndexOf("\"", startIndexOfValue);
                        if (endIndexOfValue == -1) {
                            // last attribute in tag, which is assumeed to end in ">, where represents any proxy tag delimiter
                            endIndexOfValue = tagText.Length - 2;
                        }
                        string value = tagText.Substring(startIndexOfValue, endIndexOfValue - startIndexOfValue);

                        this.AddAttribute(new ElementAttribute(attribute.ProxyName, value));

                    }
                    else {
                        this.mMarkupValidator.AddError("Missing value of attribute {0} on element {1}",
                                                      attribute.ProxyName,
                                                      this.ProxyName);
                    }
                    
                }
                else {
                    if (!attribute.IsOptional) {
                        this.mMarkupValidator.AddError("Element {0} is missing required attribute {1}",
                                                      this.ProxyName,
                                                      attribute.ProxyName);
                    }
                }
            }

            return this.ProxyName;
        }

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
            foreach(ElementAttribute attribute in this.Attributes) {
                tagBuilder.MergeAttribute(attribute.HtmlName, attribute.HtmlValue);
            }
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

        IMarkupValidator IElement.MarkupValidator {
            get {
                return this.mMarkupValidator;
            }
        }

        bool IElement.IsValid() {
            bool rootIsValid = this.mMarkupValidator.IsValid;
            if (rootIsValid) {
                foreach (IElement child in this.Children) {
                    if (!child.IsValid()) {
                        return false;
                    }
                }
            }
            return rootIsValid;
        }

        IElement IElement.Clone() {
            return new Element(this.OpenProxyTag, this.ContentContext);
        }

        IElement IElement.Truncate(int textEndIndex) {
            return (this as IElement).Truncate(textEndIndex, "...");
        }

        IElement IElement.Truncate(int textEndIndex, string textToAppend) {

            if (textEndIndex >= this.GetTextLength()) {
                return (this as IElement).Clone();
            }

            Element result = new Element(MarkupConfiguration.RootElementTagContext, string.Empty);

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

        public IEnumerable<string> ValidationErrors {
            get {
                IEnumerable<string> errors = this.mMarkupValidator.ErrorMessages;
                // add errors from children, without duplicating any error messages
                foreach(IElement child in this.Children) {
                    errors = errors.Concat(child.MarkupValidator.ErrorMessages);
                }
                return errors;
            }
        }

        public IEnumerable<string> ValidationWarnings {
            get {
                return this.mMarkupValidator.WarningMessages;
            }
        }

    }

}
