using Com.GriffithsBen.BloggableMVC.Concrete;

namespace Com.GriffithsBen.BloggableMVC.Test.Helpers {
    internal class ElementEnclosesReporter : EnclosesReporter {
        public ElementEnclosesReporter(MarkupElement element,
                                       int[] trueBetweenIndices,
                                       string value)
            : base(element, trueBetweenIndices, value) {

            this.ExpressionToTest = (x, y) => this.Element.ElementEncloses(x, y);
        }
    }
}
