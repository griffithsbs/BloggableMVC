using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.GriffithsBen.BloggableMVC.Concrete;
using Com.GriffithsBen.BloggableMVC.Abstract;
using System.Linq.Expressions;

namespace Com.GriffithsBen.BloggableMVC.Test {
    [TestClass]
    public class MarkupElementTest {

        internal class BloggableObject : IBloggable {
            public string Title { get; set; }
            public string Content { get; set; }
            public string Author { get; set; }
            public DateTime Date { get; set; }
            public string DisplayName { get; set; }
            public IEnumerable<IBloggable> Comments { get; set; }
        }

        // TODO am I going to need the bloggable in this test?
        private IBloggable BlogPost { get; set; }

        private SmartBloggable SmartBloggable {
            get {
                return new SmartBloggable(this.BlogPost);
            }
        }

        /// <summary>
        /// Helper class for testing the TagEncloses and ElementEncloses methods of the MarkupElement class
        /// </summary>
        private abstract class EnclosesReporter {
            protected MarkupElement Element { get; set; }
            /// <summary>
            /// For each tuple, the reporter will assert that the expression returns true
            /// with the given value using the indices between the two values of the tuple,
            /// including the first value and EXCLUDING the second (final) value.
            /// </summary>
            protected List<Tuple<int, int>> TrueBetweenIndices { get; set; }
            /// <summary>
            /// For each tuple, the reporter will assert that the expression returns false
            /// with the given value using the indices between the two values of the tuple,
            /// including the first value and EXCLUDING the second (final) value.
            /// </summary>
            protected List<Tuple<int, int>> FalseBetweenIndices { get; set; }
            /// <summary>
            /// An expression which invokes either MarkupElement.TagEncloses or MarkupElement.ElementIncloses
            /// </summary>
            protected Expression<Func<string, int, bool>> ExpressionToTest { get; set; }
            /// <summary>
            /// The string to test against
            /// </summary>
            protected string Value { get; set; }

            /// <summary>
            /// private constructor
            /// </summary>
            private EnclosesReporter(MarkupElement element,
                                    List<Tuple<int, int>> trueBetweenIndices, 
                                    List<Tuple<int, int>> falseBetweenIndices,
                                    string value) {

                                        this.Element = element;
                                        this.TrueBetweenIndices = trueBetweenIndices;
                                        this.FalseBetweenIndices = falseBetweenIndices;
                                        this.Value = value;
            }

            /// <summary>
            /// For convenience, the indices between which tests should be run are passed in as arrays,
            /// simply to make reporter initialisation a little less verbose
            /// </summary>
            /// <param name="element"></param>
            /// <param name="trueBetweenIndices"></param>
            /// <param name="falseBetweenIndices"></param>
            /// <param name="value"></param>
            public EnclosesReporter(MarkupElement element,
                                    int[] trueBetweenIndices,
                                    int[] falseBetweenIndices,
                                    string value)
                : this(element, new List<Tuple<int, int>>(), new List<Tuple<int, int>>(), value) {

                                        if (trueBetweenIndices.Length % 2 != 0) {
                                            throw new ArgumentException(
                                                "There must be an even number of elements in trueBetweenIndices");
                                        }
                                        if (falseBetweenIndices.Length % 2 != 0) {
                                            throw new ArgumentException(
                                                "There must be an even number of elements in falseBetweenIndices");
                                        }

                                        for (int i = 0; i < trueBetweenIndices.Length; i += 2) {
                                            this.TrueBetweenIndices.Add(Tuple.Create(i, i + 1));
                                        }
                                        for (int i = 0; i < falseBetweenIndices.Length; i += 2) {
                                            this.FalseBetweenIndices.Add(Tuple.Create(i, i + 1));
                                        }
            }
            /// <summary>
            /// Assert the tests according to the values with which the reporter instance has been instantiated
            /// </summary>
            public void Report() {
                foreach (var tuple in this.TrueBetweenIndices) {
                    for (int i = tuple.Item1; i < tuple.Item2; i++) {
                        Assert.IsTrue(this.ExpressionToTest.Compile().Invoke(this.Value, i),
                                        string.Format("{0} should return true with string value {1} and index {2}", 
                                            this.ExpressionToTest.Body.ToString(), this.Value, i));
                    }    
                }
                foreach (var tuple in this.FalseBetweenIndices) {
                    for (int i = tuple.Item1; i < tuple.Item2; i++) {
                        Assert.IsFalse(this.ExpressionToTest.Compile().Invoke(this.Value, i),
                                        string.Format("{0} should return false with string value {1} and index {2}",
                                            this.ExpressionToTest.Body.ToString(), this.Value, i));
                    }
                }
                
            }

        }

        private class TagEnclosesReporter : EnclosesReporter {
            public TagEnclosesReporter(MarkupElement element,
                                       int[] trueBetweenIndices,
                                       int[] falseBetweenIndices,
                                       string value)
                : base(element, trueBetweenIndices, falseBetweenIndices, value) {

                    this.ExpressionToTest = (x, y) => this.Element.TagEncloses(x, y);
            }

        }

        private class ElementEnclosesReporter : EnclosesReporter {
            public ElementEnclosesReporter(MarkupElement element,
                                           int[] trueBetweenIndices,
                                           int[] falseBetweenIndices,
                                           string value)
                : base(element, trueBetweenIndices, falseBetweenIndices, value) {

                this.ExpressionToTest = (x, y) => this.Element.ElementEncloses(x, y);
            }
        }


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

        [TestInitialize]
        public void TestInitialize() {
            this.BlogPost = new BloggableObject();
        }

        [TestMethod]
        public void TagEnclosesTest() {
            string value = "[p]Valid markup[/p]Valid markup";

            var reporter = new TagEnclosesReporter(element: this.pElement, 
                                                   trueBetweenIndices: new int[] {0, 19}, 
                                                   falseBetweenIndices: new int[] {19, value.Length}, 
                                                   value: value);

            value = "[p]Valid [b]markup[/b][/p]Valid markup";

            reporter = new TagEnclosesReporter(element: this.pElement,
                                               trueBetweenIndices: new int[] { 0, 26 },
                                               falseBetweenIndices: new int[] { 26, value.Length },
                                               value: value);

            reporter = new TagEnclosesReporter(element: this.bElement,
                                               trueBetweenIndices: new int[] { 9, 22 },
                                               falseBetweenIndices: new int[] { 0, 9, 22, value.Length },
                                               value: value);

        }

        [TestMethod]
        public void ElementEnclosesTest() {
            // TODO
        }

    }
}
