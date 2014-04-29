using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Com.GriffithsBen.BlogEngine.Models {
    [MetadataType(typeof(BlogEntryDataAnnotations))]
    public class BlogEntry {

        public int Id { get; set; }

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
                return BlogEntry.mGlobalSynopsisLength;
            }
            set {
                if (value < 0) {
                    throw new ArgumentException("GlobalSynopsisLength cannot be less than zero");
                }
                BlogEntry.mGlobalSynopsisLength = value;
            }
        }

        private int GetSynopsisLength() {
            if (this.SynopsisLength.HasValue) {
                return this.SynopsisLength.Value;
            }
            if (BlogEntry.GlobalSynopsisLength.HasValue) {
                return BlogEntry.GlobalSynopsisLength.Value;
            }
            return BlogEntry.DefaultSynopsisLength;
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

        public string Title { get; set; }

        public string Content { get; set; }
        
        public DateTime Date { get; set; }

        public string DisplayName { get; set; }

        public MvcHtmlString ContentHtml {
            get {
                return new MvcHtmlString(this.Content);
            }
        }

        /// <summary>
        /// The first x characters of the content string, with any HTML[/XML] tags removed and suffixed with an ellipsis.
        /// The value of x is 20 by default, but can be overridden globally or on an instance basis
        /// </summary>
        public string Synopsis {
            get {

                if (this.Content == null) {
                    throw new InvalidOperationException("Content is null");
                }

                int length = this.GetSynopsisLength();

                if (this.Content.Length <= length) {
                    return this.Content;
                }

                // strip out any HTML
                string strippedContent = Regex.Replace(this.Content, @"<[^>]*>", String.Empty);

                return strippedContent.Substring(0, length);
            }
        }

        public MvcHtmlString SynopsisHtml {
            get {
                return new MvcHtmlString(string.Format("{0}...", this.Synopsis));
            }
        }

        public IEnumerable<Comment> Comments { get; set; }

    }
}
