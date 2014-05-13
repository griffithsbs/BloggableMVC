using Com.GriffithsBen.BloggableMVC.Concrete;
using System;
using System.Linq;
using System.Reflection;

namespace Com.GriffithsBen.BloggableMVC.Test.Helpers {
    internal class EnclosesReporter : EnclosesReporterBase {
        public EnclosesReporter(MarkupElement element,
                                   int[] trueBetweenIndices,
                                   string value)
            : base(element, trueBetweenIndices, value) {

                this.MethodName = "Encloses";

                /*
                 * MarkupElement.TagEncloses is marked internal, so we use reflection to call the equivalent of
                 * this.ExpressionToTest = (x, y) => this.Element.Encloses(x, y);
                 */
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.OptionalParamBinding;
                MethodInfo enclosesMethod = typeof(MarkupElement).GetMethods(bindingFlags)
                                                                    .Where(m => m.Name == "Encloses" 
                                                                                && m.GetParameters().Count() == 2)
                                                                    .Single();
                this.ExpressionToTest = (x, y) => (bool)enclosesMethod.Invoke(this.Element, new object[] { x, y });
        }

    }
}
