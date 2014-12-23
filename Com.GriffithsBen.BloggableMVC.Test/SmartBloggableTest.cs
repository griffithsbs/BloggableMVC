using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.GriffithsBen.BloggableMVC.Abstract;
using System.Collections.Generic;
using Com.GriffithsBen.BloggableMVC.Concrete;

namespace Com.GriffithsBen.BloggableMVC.Test {
    [TestClass]
    public class SmartBloggableTest {

        internal class BloggableObject : IBloggable {
            public string Title { get; set; }
            public string Content { get; set; }
            public string Author { get; set; }
            public DateTime Date { get; set; }
            public string DisplayName { get; set; }
            public IEnumerable<IMarkupable> Comments { get; set; }
        }

        private IBloggable BlogPost { get; set; }

        private SmartBloggable SmartBloggable {
            get {
                return new SmartBloggable(this.BlogPost);
            }
        }

        [TestInitialize]
        public void TestInitialize() {
            this.BlogPost = new BloggableObject();
        }

        [TestMethod]
        public void SynopsisTest() {
            // TODO
        }

    }
}
