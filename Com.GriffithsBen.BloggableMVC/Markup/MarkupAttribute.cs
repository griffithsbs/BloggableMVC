using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    public class MarkupAttribute {

        public string ProxyName { get; private set; }

        public string HtmlName { get; private set; }

        public List<string> ValidValues { get; set; }

        public List<string> ValidParentElementNames { get; set; }

        public MarkupAttribute(string proxyName) : this(proxyName, proxyName) { }

        public MarkupAttribute(string proxyName, string htmlName) {
            this.ProxyName = proxyName;
            this.HtmlName = htmlName;
            // by default, any value is valid
            this.ValidValues = new List<string>() { "." };
            // by default, attribute is applicable to all elements
            this.ValidParentElementNames = new List<string>() { "." };
        }

    }
}
