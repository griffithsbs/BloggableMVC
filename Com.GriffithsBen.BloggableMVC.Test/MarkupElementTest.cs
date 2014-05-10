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

        }

        [TestMethod]
        public void ElementEnclosesTest() {
            string value = "[p]Valid markup[/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 3, 16 },
                                             value: value)
                                                          .Report();

            value = "[p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 3, 23 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 12, 19 },
                                             value: value)
                                                          .Report();

            value = "Valid markup[p][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 15, 16, 22, 42 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 31, 38 },
                                             value: value)
                                                          .Report();

            value = "Valid markup[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 15, 22, 29, 49 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 18, 19, 38, 44 }, 
                                             value: value)
                                                          .Report();

            value = "[quote]Valid markup[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup[/quote]";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 22, 30, 36, 56 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 25, 26, 45, 52 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[] { 7, value.Length - 7 },
                                             value: value)
                                                          .Report();

            value = "[quote]Valid[/quote]markup[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 29, 37, 43, 63 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 32, 33, 52, 59 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[] { 7, 13 },
                                             value: value)
                                                          .Report();

            value = "[quote]Valid[/quote] note space[p][b][/b][/p][p]Valid [b]markup[/b][/p]Valid markup";

            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 34, 42, 48, 68 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.bElement,
                                             trueBetweenIndices: new int[] { 37, 38, 57, 64 },
                                             value: value)
                                                          .Report();

            new ElementEnclosesReporter(element: this.quoteElement,
                                             trueBetweenIndices: new int[] { 7, 13 },
                                             value: value)
                                                          .Report();

            value = string.Empty;
            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[0],
                                             value: value)
                                                          .Report();

            value = "          [p]  [/p]";
            new ElementEnclosesReporter(element: this.pElement,
                                             trueBetweenIndices: new int[] { 13, value.Length - 3 },
                                             value: value)
                                                          .Report();
        }

    }

}
