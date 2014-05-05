using Com.GriffithsBen.BlogEngine.Abstract;
using Com.GriffithsBen.BlogEngine.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Com.GriffithsBen.BlogEngine.Concrete {
    /// <summary>
    /// An implementation of SmartBloggable intended for wrapping a blog post.
    /// 
    /// Provides a couple of extra properties specific to an entry in a blog as opposed to a comment.
    /// </summary>
    public class SmartBlogPost : SmartBloggable {

        public SmartBlogPost(IBloggable bloggable) : base(bloggable) { }

        /// <summary>
        /// The title of the blog post
        /// </summary>
        public string Title {
            get {
                return this.Bloggable.Title;
            }
            set {
                this.Bloggable.Title = value;
            }
        }

        public string DisplayName {
            get {
                return this.Bloggable.DisplayName;
            }
            set {
                this.Bloggable.DisplayName = value;
            }
        }

    }
}
