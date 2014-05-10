using Com.GriffithsBen.BloggableMVC.Concrete;

namespace Com.GriffithsBen.BloggableMVC.Test.Helpers {
    internal class TagEnclosesReporter : EnclosesReporter {
        public TagEnclosesReporter(MarkupElement element,
                                   int[] trueBetweenIndices,
                                   string value)
            : base(element, trueBetweenIndices, value) {

            this.ExpressionToTest = (x, y) => this.Element.TagEncloses(x, y);
        }

    }
}
