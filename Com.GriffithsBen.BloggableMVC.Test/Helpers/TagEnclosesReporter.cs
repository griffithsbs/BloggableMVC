using Com.GriffithsBen.BloggableMVC.Concrete;
using System;
using System.Reflection;

namespace Com.GriffithsBen.BloggableMVC.Test.Helpers {
    internal class TagEnclosesReporter : EnclosesReporter {
        public TagEnclosesReporter(MarkupElement element,
                                   int[] trueBetweenIndices,
                                   string value)
            : base(element, trueBetweenIndices, value) {

                this.MethodName = "TagEncloses";

                /*
                 * MarkupElement.TagEncloses is marked internal, so we use reflection to call the equivalent of
                 * this.ExpressionToTest = (x, y) => this.Element.TagEncloses(x, y);
                 */
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                MethodInfo tagEnclosesMethod = typeof(MarkupElement).GetMethod("TagEncloses", bindingFlags);
                this.ExpressionToTest = (x, y) => (bool)tagEnclosesMethod.Invoke(this.Element, new object[] { x, y });
        }

    }
}
