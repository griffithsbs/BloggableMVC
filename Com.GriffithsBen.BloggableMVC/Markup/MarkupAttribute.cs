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

        public MarkupAttribute(string proxyName) {
            this.ProxyName = proxyName;
            // TODO set html name
            this.ValidValues = new List<string>();
            this.ValidParentElementNames = new List<string>();
        }

    }
}
