using Com.GriffithsBen.BloggableMVC.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.GriffithsBen.BloggableMVC.Markup {
    /// <summary>
    /// The definition of an element to be used in marking up blog content, consisting of a proxy element to be used by the user
    /// and an HTML element with which the proxy element can be replaced in order to display the marked up content
    /// as HTML.
    /// It is assumed that all element are non-self-closing, i.e. both proxy and HTML element consist of a pair
    /// of tags to open and close the element.
    /// </summary>
    public class MarkupElement {

        /// <summary>
        /// The name of the element to be used when marking up blog content by the user
        /// e.g. "b" or "quote"
        /// </summary>
        public string ProxyElement { get; private set; }

        /// <summary>
        /// The name of the HTML element with which the proxy element will be replaced
        /// </summary>
        public string HtmlElement { get; private set; }

        public List<MarkupAttribute> ValidAttributes { get; private set; }

        public IEnumerable<MarkupAttribute> MandatoryAttributes { get; private set; }

        public MarkupElement(string proxyElementName, string htmlElementName, IEnumerable<MarkupAttribute> mandatoryAttributes = null) {
            this.ProxyElement = proxyElementName;
            this.HtmlElement = htmlElementName;
            this.ProxyTagDelimiter = MarkupConfiguration.ProxyTagDelimiter;
            this.MandatoryAttributes = mandatoryAttributes;
            this.ValidAttributes = MarkupConfiguration.GetValidAttributesForElement(this.ProxyElement);
            if (this.MandatoryAttributes != null) {
                this.ValidAttributes.AddRange(this.MandatoryAttributes);
            }
        }

        internal string OpenProxyTagRegexFactor {
            get {
                StringBuilder builder = new StringBuilder();

                builder.AppendFormat(@"{0}{1}", 
                                     MarkupConfiguration.ProxyTagDelimiter.GetEscapedOpeningCharacter(),
                                     this.ProxyElement
                );

                if (this.ValidAttributes != null) {

                    foreach (MarkupAttribute attribute in this.ValidAttributes) {
                        // one or more space characters if not all attributes are optional and this is the first attribute, 
                        // otherwise zero or more space characters
                        string spaceRequirement = 
                            attribute == this.ValidAttributes.First() && this.ValidAttributes.Any(x => !x.IsOptional) ? "+" : "*";
                        builder.Append(@"[\s]" + spaceRequirement);

                        if (attribute.IsOptional) {
                            builder.Append("(");
                        }

                        builder.AppendFormat("{0}=", attribute.ProxyName);

                        if (attribute.ValidValues.Count == 1) {
                            builder.AppendFormat("\"{0}\"", attribute.ValidValues.Single());
                        }
                        else {
                            builder.Append("\"(");
                            foreach (string validValue in attribute.ValidValues) {
                                builder.AppendFormat("{0}", validValue);
                                if (validValue != attribute.ValidValues.Last()) {
                                    builder.Append("|");
                                }
                            }
                            builder.Append(")\"");
                        }

                        if (attribute.IsOptional) {
                            builder.Append(")?");
                        }

                        // zero or more space characters
                        builder.Append(@"[\s]*");
                    }

                }
                
                builder.AppendFormat(@"[\s]*{0}", MarkupConfiguration.ProxyTagDelimiter.GetEscapedClosingCharacter());

                return builder.ToString();
            }
        }

        private string OpenProxyTagFormat {
            get {
                return this.ProxyTagDelimiter.GetOpenTagFormat();
            }
        }
         
        private string CloseProxyTagFormat {
            get {
                return this.ProxyTagDelimiter.GetCloseTagFormat();
            }
        }

        public ProxyTagDelimiter ProxyTagDelimiter { get; set; }

        /// <summary>
        /// See ProxyTagDelimiterExtensions class for supported tag formats
        /// </summary>
        public string OpenProxyTag {
            get {
                return string.Format(this.OpenProxyTagFormat, this.ProxyElement);
            }
        }

        /// <summary>
        /// See ProxyTagDelimiterExtensions class for supported tag formats
        /// </summary>
        public string CloseProxyTag {
            get {
                return string.Format(this.CloseProxyTagFormat, this.ProxyElement);
            }
        }

    }
}
