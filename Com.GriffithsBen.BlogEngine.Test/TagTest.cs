using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.GriffithsBen.BlogEngine.Concrete;
using Com.GriffithsBen.BlogEngine.Abstract;

namespace Com.GriffithsBen.BlogEngine.Test {
    [TestClass]
    public class TagTest {

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

        [TestInitialize]
        public void TestInitialize() {
            this.BlogPost = new BloggableObject();
        }

        [TestMethod]
        public void TagEnclosesTest() {
            // TODO
        }

        [TestMethod]
        public void ElementEnclosesTest() {
            // TODO
        }

    }
}
