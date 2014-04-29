
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Com.GriffithsBen.BlogEngine.Models {
    public class Comment {

        public int Id { get; set; }

        public string Author { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }

        public MvcHtmlString ContentHtml {
            get {
                return new MvcHtmlString(this.Content);
            }
        }

        // TODO
        public IEnumerable<Comment> Replies { get; set; }
    }
}
