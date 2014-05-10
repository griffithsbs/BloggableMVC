using Com.GriffithsBen.BloggableMVC.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Com.GriffithsBen.BloggableMVC.Test.Helpers {
    /// <summary>
    /// Helper class for testing the TagEncloses and ElementEncloses methods of the MarkupElement class
    /// </summary>
    internal abstract class EnclosesReporter {
        protected MarkupElement Element { get; set; }
        /// <summary>
        /// For each tuple, the reporter will assert that the expression returns true
        /// with the given value using the indices between the two values of the tuple,
        /// including the first value and EXCLUDING the second (final) value.
        /// For all other possible indices in the given value, the reporter will assert that the 
        /// expression returns false.
        /// </summary>
        protected List<Tuple<int, int>> TrueBetweenIndices { get; set; }

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
                                string value) {

            this.Element = element;
            this.TrueBetweenIndices = trueBetweenIndices;
            this.Value = value;
        }

        /// <summary>
        /// For convenience, the indices between which tests should be run are passed in as an array,
        /// simply to make reporter initialisation a little less verbose
        /// </summary>
        public EnclosesReporter(MarkupElement element,
                                int[] trueBetweenIndices,
                                string value)
            : this(element, new List<Tuple<int, int>>(), value) {

            if (trueBetweenIndices.Length % 2 != 0) {
                throw new ArgumentException(
                    "There must be an even number of elements in trueBetweenIndices");
            }

            for (int i = 0; i < trueBetweenIndices.Length; i += 2) {
                this.TrueBetweenIndices.Add(
                    Tuple.Create(trueBetweenIndices[i], trueBetweenIndices[i + 1])
                );
            }
        }
        /// <summary>
        /// Assert the tests according to the values with which the reporter instance has been instantiated
        /// </summary>
        public void Report() {

            List<int> falseBetweenIndices = new List<int>();
            for (int i = 0; i < this.Value.Length; i++) {
                falseBetweenIndices.Add(i);
            }

            foreach (var tuple in this.TrueBetweenIndices) {
                for (int i = tuple.Item1; i < tuple.Item2; i++) {
                    Assert.IsTrue(this.ExpressionToTest.Compile().Invoke(this.Value, i),
                                    string.Format("{0} should return true with element {1}, value \"{2}\" and index {3}",
                                        this.ExpressionToTest.Body.ToString(),
                                        this.Element.ProxyElement,
                                        this.Value,
                                        i));

                    falseBetweenIndices.Remove(i);
                }
            }

            foreach (int index in falseBetweenIndices) {
                Assert.IsFalse(this.ExpressionToTest.Compile().Invoke(this.Value, index),
                                string.Format("{0} should return false with element {1}, value \"{2}\" and index {3}",
                                this.ExpressionToTest.Body.ToString(),
                                this.Element.ProxyElement,
                                this.Value,
                                index));
            }

        }

    }
}
