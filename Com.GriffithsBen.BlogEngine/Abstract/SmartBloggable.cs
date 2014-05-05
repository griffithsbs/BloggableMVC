﻿using Com.GriffithsBen.BlogEngine.Concrete;
using Com.GriffithsBen.BlogEngine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.GriffithsBen.BlogEngine.Abstract {
    /// <summary>
    /// SmartBloggable
    /// Decorates the IBloggable interface with general behaviour for marking up the content of both blog posts
    /// and comments
    /// </summary>
    public class SmartBloggable {

        /// <summary>
        /// The instance of IBloggable being wrapped and decorated with smart blog behaviour
        /// We might expect this to be a domain model, DAO, or view model
        /// </summary>
        public IBloggable Bloggable { get; set; }

        public SmartBloggable(IBloggable bloggable) {
            this.Bloggable = bloggable;
            this.TagCollection = TagConfiguration.CopyTagCollection();
        }

        public IEnumerable<Tag> TagCollection { get; private set; }

        /// <summary>
        /// Adds a new tag to the tag collection for this blog entry
        /// </summary>
        /// <param name="proxyElementName"></param>
        /// <param name="htmlElementName"></param>
        public void AddTag(string proxyElementName, string htmlElementName) {
            this.TagCollection.Concat(new List<Tag>() { new Tag(proxyElementName, htmlElementName) });
        }

        /// <summary>
        /// Removes any Tag instances with the given ProxyElement name from the TagCollection
        /// </summary>
        /// <param name="proxyElementName">the name of the ProxyElement of the tag to be removed</param>
        /// <returns></returns>
        public void RemoveTag(string proxyElementName) {
            this.TagCollection = this.TagCollection.Except(
                this.TagCollection.Where(x => x.ProxyElement == proxyElementName)
            );
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
        /// Content may include tags defined in the BlogEntry's TagCollection member 
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

        /// <summary>
        /// The Content string, with all instances of tags defined in the BlogEntry's TagCollection converted
        /// into Html tags
        /// </summary>
        private string EncodedHtmlContent {
            get {
                if(this.Content == null) {
                    throw new InvalidOperationException("BlogEntry's Content string is null");
                }

                if (this.TagCollection == null) {
                    return this.Content;
                }

                string result = this.Content;
                foreach (Tag tag in this.TagCollection) {
                    result = tag.ReplaceProxyWithHtml(result);
                }
                return result;
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
                return new MvcHtmlString(this.EncodedHtmlContent);
            }
        }

        /// <summary>
        /// The first x characters of the content string, with any tags removed and suffixed with an ellipsis.
        /// The value of x is 20 by default, but can be overridden globally or on an instance basis
        /// </summary>
        public string Synopsis {
            get {

                if (this.Content == null) {
                    return this.Content;
                }

                string result = this.Content;

                if (this.TagCollection != null) {
                    foreach (Tag tag in this.TagCollection) {
                        result = tag.RemoveProxyTags(result);
                    }
                }
                
                int length = this.GetSynopsisLength();

                if (result.Length <= length) {
                    return result;
                }
                return result.Substring(0, length);
            }
        }

        public MvcHtmlString SynopsisHtml {
            get {
                return new MvcHtmlString(string.Format("{0}...", this.Synopsis));
            }
        }

    }
}