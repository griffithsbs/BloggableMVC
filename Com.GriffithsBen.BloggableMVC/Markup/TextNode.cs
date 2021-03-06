﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    /// <summary>
    /// An instance of a special case of a markup element: one which contains only text
    /// </summary>
    internal class TextNode : IElement {

        private string Text { get; set; }

        public TextNode(string text) {
            this.mMarkupValidator = new MarkupValidator();
            this.Text = text;
        }

        public override string ToString() {
            return this.Text;
        }

        private MarkupValidator mMarkupValidator { get; set; }

        IMarkupValidator IElement.MarkupValidator {
            get {
                return this.mMarkupValidator;
            }
        }

        public IElement Clone() {
            return new TextNode(this.Text);
        }

        int IElement.GetTextLength() {
            return this.Text.Length;
        }

        MvcHtmlString IElement.GetHtml() {
            return new MvcHtmlString(this.Text);
        }

        bool IElement.IsValid() {
            return this.mMarkupValidator.IsValid;
        }

        IElement IElement.Truncate(int textEndIndex) {
            return (this as IElement).Truncate(textEndIndex, "...");
        }

        IElement IElement.Truncate(int textEndIndex, string textToAppend) {
            if (textEndIndex >= this.Text.Length) {
                return this.Clone();
            }
            return new TextNode(string.Format("{0}{1}", this.Text.Substring(0, textEndIndex), textToAppend));
        }

        public IEnumerable<string> ValidationErrors {
            get {
                return this.mMarkupValidator.ErrorMessages;
            }
        }

        public IEnumerable<string> ValidationWarnings {
            get {
                return this.mMarkupValidator.WarningMessages;
            }
        }

    }

}
