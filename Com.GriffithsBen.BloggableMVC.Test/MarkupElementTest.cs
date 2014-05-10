using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.GriffithsBen.BloggableMVC.Concrete;
using Com.GriffithsBen.BloggableMVC.Abstract;

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

        private IBloggable BlogPost { get; set; }

        private SmartBloggable SmartBloggable {
            get {
                return new SmartBloggable(this.BlogPost);
            }
        }

        private MarkupElement pElement {
            get {
                return new MarkupElement("p", "p");
            }
        }

        [TestInitialize]
        public void TestInitialize() {
            this.BlogPost = new BloggableObject();
        }

        [TestMethod]
        public void TagEnclosesTest() {
            string value = "[p]Valid markup[/p]Valid markup";

            for (int i = 0; i < 19; i++) {
                Assert.IsTrue(this.pElement.TagEncloses(value, i), string.Format("p tag should enclose index {0} of string \"{1}\"", i, value));
            }
            for (int i = 19; i < value.Length; i++) {
                Assert.IsFalse(this.pElement.TagEncloses(value, i), string.Format("p tag should not enclose index {0} of string \"{1}\"", i, value));
            }
        }

        [TestMethod]
        public void ElementEnclosesTest() {
            // TODO
        }

    }
}
