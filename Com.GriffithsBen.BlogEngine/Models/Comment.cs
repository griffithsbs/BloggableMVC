
using System;
using System.Web.Mvc;

namespace Com.GriffithsBen.BlogEngine.Models {
    public class Comment {

        public string Author { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }

        public MvcHtmlString ContentHtml {
            get {
                return new MvcHtmlString(this.Content);
            }
        }
    }
}
