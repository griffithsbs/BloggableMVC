using Com.GriffithsBen.BloggableMVC.Abstract;
using Com.GriffithsBen.BloggableMVC.Concrete;
using Com.GriffithsBen.BloggableMVC.Configuration;
using Com.GriffithsBen.BloggableMVC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IBloggable Bloggable { get; set; }

        public SmartBloggable(IBloggable bloggable) {
            this.Bloggable = bloggable;
            this.MarkupElements = MarkupConfiguration.CopyMarkupElements();
            this.ModelData = new Dictionary<string, string>();
        }

        public IEnumerable<MarkupElement> MarkupElements { get; private set; }

        /// <summary>
        /// Adds a new element to the MarkupElement collection for this blog entry
        /// </summary>
        /// <param name="proxyElementName"></param>
        /// <param name="htmlElementName"></param>
        public void AddMarkupElement(string proxyElementName, string htmlElementName) {
            this.MarkupElements.Concat(new List<MarkupElement>() { new MarkupElement(proxyElementName, htmlElementName) });
        }

        /// <summary>
        /// Removes any MarkupElement instances with the given ProxyElement name from the MarkupElements collection
        /// </summary>
        /// <param name="proxyElementName">the name of the ProxyElement of the element to be removed</param>
        /// <returns></returns>
        public void RemoveMarkupElement(string proxyElementName) {
            this.MarkupElements = this.MarkupElements.Except(
                this.MarkupElements.Where(x => x.ProxyElement == proxyElementName)
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

        /// <summary>
        /// The Content string, with all instances of tags defined in the BlogEntry's MarkupElements collection converted
        /// into Html tags
        /// </summary>
        private string EncodedHtmlContent {
            get {
                if(this.Content == null) {
                    throw new InvalidOperationException("BlogEntry's Content string is null");
                }

                if (this.MarkupElements == null) {
                    return this.Content;
                }

                string result = this.Content;
                foreach (MarkupElement element in this.MarkupElements) {
                    result = element.ReplaceProxyWithHtml(result);
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
                return new MvcHtmlString(this.EncodedHtmlContent);
            }
        }

        /// <summary>
        /// The first x characters of the Content string, excluding tags
        /// </summary>
        private string Synopsis {
            get {

                if (this.Content == null) {
                    return this.Content;
                }

                string result = this.Content;

                int length = this.GetSynopsisLength();

                if (result.Length <= length) {
                    return result;
                }

                // find out which, if any, tag will be 'broken' by truncating the string
                // i.e. does the end of the substring to be taken fall inside an instance of one of the tags?
                MarkupElement brokenTag = null;

                // also find out which, if any, elements will be 'broken' by truncating the string
                // i.e. does the end of the substring to be taken fall inside an instance of one or more of the elements?
                List<MarkupElement> brokenElements = new List<MarkupElement>();

                if (this.MarkupElements != null) {
                    foreach (MarkupElement element in this.MarkupElements) {
                        if (element.TagEncloses(result, length - 1)) {

                            if (element.ElementEncloses(result, length - 1)) {
                                brokenElements.Add(element);
                            }
                            else {
                                brokenTag = element;
                            }
                        }
                    }
                }

                result = result.Substring(0, length);

                // if there is a broken tag on the end of the result, fix it
                // TODO it could be a start tag or an end tag that is broken
                // if a start tag, it needs to be removed rather than fixed
                if (brokenTag != null) {
                    int startOfBrokenTag = result.LastIndexOf('[');
                    result = result.Substring(0, startOfBrokenTag);
                    result = brokenTag.AppendProxyEndTagTo(result);
                }
                
                // add a closing tag for each of the broken elements
                // TODO this isn't necessarily going to result in valid HTML
                // we need to test whether the tags are balanced
                foreach (MarkupElement element in brokenElements) {
                    result = string.Format("{0}{1}", result, element.ProxyElement.GetClosingProxyTag());
                }
                
                return result.Substring(0, length);
            }
        }

        /// <summary>
        /// The Synopsis string, with all instances of tags defined in the BlogEntry's MarkupElements collection converted
        /// into Html tags
        /// </summary>
        private string EncodedHtmlSynopsis {
            get {
                if (this.Synopsis == null) {
                    throw new InvalidOperationException("BlogEntry's Content string is null");
                }

                if (this.MarkupElements == null) {
                    return this.Synopsis;
                }

                string result = this.Synopsis;
                foreach (MarkupElement element in this.MarkupElements) {
                    result = element.ReplaceProxyWithHtml(result);
                }
                return result;
            }
        }

        public MvcHtmlString SynopsisHtml {
            get {
                return new MvcHtmlString(string.Format("{0}...", this.EncodedHtmlSynopsis));
            }
        }

        /// <summary>
        /// A Dictionary of key-value pairs of strings, useful if wish to store extra properties from our 
        /// wrapped object that are not part of the IBloggable interface which the wrapped object implements
        /// </summary>
        public Dictionary<string, string> ModelData { get; set; }

    }
}
