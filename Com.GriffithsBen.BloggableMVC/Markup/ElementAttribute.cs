using Com.GriffithsBen.BloggableMVC.Configuration;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    /// <summary>
    /// An instance of an attribute on a markup element in a parsed content string
    /// </summary>
    public class ElementAttribute {

        public string ProxyName { get; private set; }

        public string HtmlName {
            get {
                return MarkupConfiguration.GetHtmlNameForAttribute(this.ProxyName);
            }
        }

        public string ProxyValue { get; set; }

        public string HtmlValue {
            get {
                // TODO is there ever a use case for HtmlValue != ProxyValue ?
                return this.ProxyValue;
            }
        }

        public ElementAttribute(string proxyName) : this(proxyName, string.Empty) { }

        public ElementAttribute(string proxyName, string proxyValue) {
            this.ProxyName = proxyName;
            this.ProxyValue = proxyValue;
        }
    }
}
