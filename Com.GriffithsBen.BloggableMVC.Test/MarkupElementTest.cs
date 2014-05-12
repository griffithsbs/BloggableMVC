using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.GriffithsBen.BloggableMVC.Concrete;
using Com.GriffithsBen.BloggableMVC.Abstract;
using System.Linq.Expressions;
using Com.GriffithsBen.BloggableMVC.Test.Helpers;

namespace Com.GriffithsBen.BloggableMVC.Test {
    [TestClass]
    public class MarkupElementTest {

        private MarkupElement pElement {
            get {
                return new MarkupElement("p", "p");
            }
        }

        private MarkupElement bElement {
            get {
                return new MarkupElement("b", "em");
            }
        }

        private MarkupElement quoteElement {
            get {
                return new MarkupElement("quote", "blockquote");
            }
        }

        [TestMethod]
        public void TagEnclosesTest() {
            string value = "[p]Valid markup[/p]Valid markup";

            new TagEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] {0, 19},
                                             value: value)
                                                          .Report();

            value = "[p]Valid [b]markup[/b][/p]Valid markup";

            new TagEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 0, 26 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 9, 22 },
                                             value: value)
                                                          .Report();

            value = "Valid markup[p][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new TagEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 12, 19, 19, 45 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 28, 41 },
                                             value: value)
                                                          .Report();

            value = "Valid markup[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new TagEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 12, 52 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 15, 22, 35, 48 },
                                             value: value)
                                                          .Report();

            value = "[quote]Valid markup[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup[/quote]";

            new TagEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 19, 59 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 22, 29, 42, 55 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[] { 0, value.Length },
                                             value: value)
                                                          .Report();

            value = "[quote]Valid[/quote]markup[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new TagEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 26, 66 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 29, 36, 49, 62 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[] { 0, 20 },
                                             value: value)
                                                          .Report();

            value = "[quote]Valid[/quote] note space[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new TagEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 31, 71 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 34, 41, 54, 67 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[] { 0, 20 },
                                             value: value)
                                                          .Report();

            value = string.Empty;
            new TagEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[0],
                                             value: value)
                                                          .Report();

            value = "          [p]  [/p]";
            new TagEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 10, value.Length },
                                             value: value)
                                                          .Report();


            value = "Invalid[/quote] note space[p][b][/b][/p]Invalid [b]markup[/b][/p]Invalid markup";

            new TagEnclosesReporter(element: this.pElement,
                                                  trueBetweenIndices: new int[] { 26, 40 },
                                                  value: value)
                                                               .Report();

            new TagEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 29, 36, 48, 61 },
                                             value: value)
                                                          .Report();

            new TagEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[0],
                                             value: value)
                                                          .Report();

            value = "[p]Valid [b]markup[/b][/p]Invalid markup[/p]";

            new TagEnclosesReporter(element: this.pElement,
                                                 trueBetweenIndices: new int[] { 0, 26 },
                                                 value: value)
                                                              .Report();

            new TagEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 9, 22 },
                                             value: value)
                                                          .Report();

        }

        [TestMethod]
        public void ElementEnclosesTest() {
            string value = "[p]Valid markup[/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 3, 15 },
                                             value: value)
                                                          .Report();

            value = "[p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 3, 22 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 12, 18 },
                                             value: value)
                                                          .Report();

            value = "Valid markup[p][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 22, 41 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 31, 37 },
                                             value: value)
                                                          .Report();

            value = "Valid markup[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 15, 22, 29, 48 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 38, 44 }, 
                                             value: value)
                                                          .Report();

            value = "[quote]Valid markup[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup[/quote]";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 22, 29, 36, 55 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 45, 51 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[] { 7, value.Length - 8 },
                                             value: value)
                                                          .Report();

            value = "[quote]Valid[/quote]markup[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 29, 36, 43, 62 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 52, 58 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[] { 7, 12 },
                                             value: value)
                                                          .Report();

            value = "[quote]Valid[/quote] note space[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 34, 41, 48, 67 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 57, 63 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[] { 7, 12 },
                                             value: value)
                                                          .Report();

            value = string.Empty;
            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[0],
                                             value: value)
                                                          .Report();

            value = "          [p]  [/p]";
            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 13, value.Length - 4 },
                                             value: value)
                                                          .Report();


            value = "Invalid[/quote] note space[p][b][/b][/p]Invalid [b]markup[/b][/p]Invalid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 29, 36  },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 51, 57 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[0],
                                             value: value)
                                                          .Report();

            value = "[p]Valid [b]markup[/b][/p]Invalid markup[/p]";

            new ElementEnclosesReporter(element: this.pElement,
                                                 trueBetweenIndices: new int[] { 3, 22 },
                                                 value: value)
                                                              .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 12, 18 },
                                             value: value)
                                                          .Report();

        }

        [TestMethod]
        public void ReplaceProxyWithHtmlTest() {

            string value = "[p]Valid markup[/p]Valid markup";
            string expected = "<p>Valid markup</p>Valid markup";
            Assert.AreEqual(expected, this.pElement.ReplaceProxyWithHtml(value));

            value = "[quote][p]Valid markup[/p]Valid markup[/quote][p]valid markup[b]valid markup [/b][/p][b][/b]";
            expected = "<blockquote><p>Valid markup</p>Valid markup</blockquote><p>valid markup<em>valid markup </em></p><em></em>";
            Assert.AreEqual(expected, this.bElement.ReplaceProxyWithHtml(
                                        this.quoteElement.ReplaceProxyWithHtml(
                                            this.pElement.ReplaceProxyWithHtml(value)
                                       ))
            );

            value = "[quotp]Invalid markup[/p]Invalid markup[/quote][p]Invalid markupInvalid markup [/b][b[/b]";
            expected = "[quotp]Invalid markup</p>Invalid markup</blockquote><p>Invalid markupInvalid markup </em>[b</em>";
            Assert.AreEqual(expected, this.bElement.ReplaceProxyWithHtml(
                                        this.quoteElement.ReplaceProxyWithHtml(
                                            this.pElement.ReplaceProxyWithHtml(value)
                                       ))
            );
        }

    }

}
