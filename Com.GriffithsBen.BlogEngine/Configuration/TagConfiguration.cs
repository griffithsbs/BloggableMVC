using Com.GriffithsBen.BlogEngine.Models;
using System;
using System.Collections.Generic;

namespace Com.GriffithsBen.BlogEngine.Configuration {

    /// <summary>
    /// 
    /// </summary>
    public static class TagConfiguration {

        static TagConfiguration() {
            TagConfiguration.TagCollection = TagConfiguration.DefaultTagCollection;
        }

        private static List<Tag> DefaultTagCollection = new List<Tag>() {
            new Tag("b", "em"),
            new Tag("i", "i"),
            new Tag("p", "p"),
            new Tag("quote", "blockquote")
        };

        /// <summary>
        /// The static collection of tags that will be used to initialise the tag collection
        /// of all new blog entries
        /// </summary>
        private static List<Tag> TagCollection { get; set; }

        /// <summary>
        /// Used to get hold of a copy of the tag collection which can then be changed independently
        /// of the default collection of tags
        /// </summary>
        /// <returns>a new collection of tags, cloned from TagConfiguration.TagCollection</returns>
        public static IEnumerable<Tag> CopyTagCollection() {
            if (TagConfiguration.TagCollection == null) {
                throw new InvalidOperationException("TagCollection is null");
            }
            Tag[] copy = new Tag[TagConfiguration.TagCollection.Count];

            if(copy.Length > 0) {
                TagConfiguration.TagCollection.ToArray().CopyTo(copy, 0);
            }
            return copy;
        }

        public static IEnumerable<Tag> GetTagCollection() {
            return TagConfiguration.TagCollection;
        }

        /// <summary>
        /// Adds a new tag to the tag collection
        /// </summary>
        /// <param name="proxyElementName"></param>
        /// <param name="htmlElementName"></param>
        public static void AddTag(string proxyElementName, string htmlElementName) {

            if (proxyElementName == null) {
                throw new ArgumentException("proxyElementName is null");
            }
            if (htmlElementName == null) {
                throw new ArgumentException("htmlElementName is null");
            }
            if (string.IsNullOrWhiteSpace(proxyElementName)) {
                throw new ArgumentException("proxyElementName is white space");
            }
            if (string.IsNullOrWhiteSpace(htmlElementName)) {
                throw new ArgumentException("htmlElementName is white space");
            }

            TagConfiguration.TagCollection.Add(new Tag(proxyElementName, htmlElementName));
        }

        /// <summary>
        /// Removes any Tag instances with the given ProxyElement name from the TagCollection
        /// </summary>
        /// <param name="proxyElementName">the name of the ProxyElement of the tag to be removed</param>
        /// <returns></returns>
        public static void RemoveTag(string proxyElementName) {

            if (proxyElementName == null) {
                throw new ArgumentException("proxyElementName is null");
            }
            
            Tag targetTag = TagConfiguration.TagCollection.Find(x => x.ProxyElement == proxyElementName);
            TagConfiguration.TagCollection.Remove(targetTag);
        }

    }
}
