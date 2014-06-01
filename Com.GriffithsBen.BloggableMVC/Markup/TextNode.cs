using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    // TODO rather than having TextNode derive from Element, both should implement an IElement interface

    internal class TextNode : Element {

        private string Text { get; set; }

        public TextNode(string text) {
            this.Text = text;
        }

        public override string ToString() {
            return this.Text;
        }

        public override int GetTextLength() {
            return this.Text.Length;
        }

        public override Element Truncate(int textEndIndex, string textToAppend = "...") {
            if (textEndIndex >= this.GetTextLength()) {
                return this.Clone();
            }
            return new TextNode(string.Format("{0}{1}", this.Text.Substring(0, textEndIndex), textToAppend));
        }

        public override MvcHtmlString GetHtml() {
            return new MvcHtmlString(this.Text);
        }

        internal override Element Clone() {
            return new TextNode(this.Text);
        }

    }
}
