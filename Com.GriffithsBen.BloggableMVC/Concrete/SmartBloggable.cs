using Com.GriffithsBen.BloggableMVC.Abstract;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Concrete {

    /// <summary>
    /// SmartBloggable
    /// Decorates the IBloggable interface with general behaviour for marking up 
    /// blog content (e.g. blog posts or comments)
    /// </summary>
    public class SmartBloggable {

        /// <summary>
        /// The instance of IBloggable being wrapped and decorated with smart blog behaviour
        /// We might expect this to be a domain model, DAO, or view model
        /// </summary>
        private IBloggable Bloggable { get; set; }

        /// <summary>
        /// SmartBloggable is composed with a Markupable object to which it can delegate markup-specific calls
        /// </summary>
        private Markupable Markupable { get; set; }

        public SmartBloggable(IBloggable bloggable) {
            this.Bloggable = bloggable;
            this.Markupable = new Markupable(bloggable);
            this.ModelData = new Dictionary<string, string>();
        }

        /// <summary>
        /// Note that this is the maximum number of characters to be returned in the synopsis of the content, 
        /// excluding the 3 characters that make up the trailing ellipsis
        /// </summary>
        private const int DefaultSynopsisLength = 20;

        private static int? mGlobalSynopsisLength;
        /// <summary>
        /// If set, overrides the default synopsis length
        /// </summary>
        public static int? GlobalSynopsisLength {
            get {
                return SmartBloggable.mGlobalSynopsisLength;
            }
            set {
                if (value < 0) {
                    throw new ArgumentException("GlobalSynopsisLength cannot be less than zero");
                }
                SmartBloggable.mGlobalSynopsisLength = value;
            }
        }

        private int GetSynopsisLength() {
            if (this.SynopsisLength.HasValue) {
                return this.SynopsisLength.Value;
            }
            if (SmartBloggable.GlobalSynopsisLength.HasValue) {
                return SmartBloggable.GlobalSynopsisLength.Value;
            }
            return SmartBloggable.DefaultSynopsisLength;
        }

        private int? mSynopsisLength;
        /// <summary>
        /// If set, overrides the default or global synopsis length.
        /// </summary>
        public int? SynopsisLength {
            get {
                return this.mSynopsisLength;
            }
            set {
                if (value < 0) {
                    throw new ArgumentException("SynopsisLength cannot be less than zero");
                }
                this.mSynopsisLength = value;
            }
        }

        /// <summary>
        /// The content of a blog post or comment
        /// Content may include tags defined in the BlogEntry's MarkupElements member 
        /// </summary>
        public string Content {
            get {
                return this.Bloggable.Content;
            }
            set {
                this.Bloggable.Content = value;
            }
        }

        /// <summary>
        /// The name of the author of the bloggable content
        /// </summary>
        public string Author {
            get {
                return this.Bloggable.Author;
            }
            set {
                this.Bloggable.Author = value;
            }
        }

        public DateTime Date {
            get {
                return this.Bloggable.Date;
            }
            set {
                this.Bloggable.Date = value;
            }
        }

        /// <summary>
        /// The title of the blog content
        /// </summary>
        public string Title {
            get {
                return this.Bloggable.Title;
            }
            set {
                this.Bloggable.Title = value;
            }
        }

        /// <summary>
        /// A display name for the blog content, for example for use in a pretty URL
        /// </summary>
        public string DisplayName {
            get {
                return this.Bloggable.DisplayName;
            }
            set {
                this.Bloggable.DisplayName = value;
            }
        }

        /// <summary>
        /// In the case of a Blog post, this is the collection of comments on that post.
        /// In the case of a comment, this is the collection of replies to that comment.
        /// </summary>
        public IEnumerable<IBloggable> Comments {
            get {
                return this.Bloggable.Comments;
            }
            set {
                this.Bloggable.Comments = value;
            }
        }

        public MvcHtmlString ContentHtml {
            get {
                return this.Markupable.ContentHtml;
            }
        }

        public MvcHtmlString SynopsisHtml {
            get {
                return this.Markupable.TruncateContentHtml(this.GetSynopsisLength());
            }
        }

        /// <summary>
        /// A Dictionary of key-value pairs of strings, useful if wish to store extra properties from our 
        /// wrapped object that are not part of the IBloggable interface which the wrapped object implements
        /// </summary>
        public Dictionary<string, string> ModelData { get; set; }

    }
}
