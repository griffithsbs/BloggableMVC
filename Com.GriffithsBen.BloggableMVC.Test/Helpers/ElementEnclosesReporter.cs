using Com.GriffithsBen.BloggableMVC.Concrete;
using System.Reflection;

namespace Com.GriffithsBen.BloggableMVC.Test.Helpers {
    internal class ElementEnclosesReporter : EnclosesReporter {
        public ElementEnclosesReporter(MarkupElement element,
                                       int[] trueBetweenIndices,
                                       string value)
            : base(element, trueBetweenIndices, value) {

                this.MethodName = "ElementEncloses";

                /*
                 * MarkupElement.ElementEncloses is marked internal, so we use reflection to call the equivalent of
                 * this.ExpressionToTest = (x, y) => this.Element.ElementEncloses(x, y);
                 */
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                MethodInfo elementEnclosesMethod = typeof(MarkupElement).GetMethod("ElementEncloses", bindingFlags);
                this.ExpressionToTest = (x, y) => (bool)elementEnclosesMethod.Invoke(this.Element, new object[] { x, y });

        }
    }
}
