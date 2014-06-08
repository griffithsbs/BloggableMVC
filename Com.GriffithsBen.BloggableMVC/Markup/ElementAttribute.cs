using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    public class ElementAttribute {

        public string ProxyName { get; private set; }

        public string HtmlName { get; private set; }

        public string ProxyValue { get; set; }

        public string HtmlValue { get; set; }

        public ElementAttribute(string proxyName) : this(proxyName, string.Empty) { }

        public ElementAttribute(string proxyName, string proxyValue) {
            this.ProxyName = proxyName;
            this.ProxyValue = proxyValue;
        }
    }
}
