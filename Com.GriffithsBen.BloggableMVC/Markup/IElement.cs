using System.Collections.Generic;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Markup {
    /// <summary>
    /// Defines the behaviour that an instance of a markup element in a parsed content string possesses
    /// </summary>
    public interface IElement {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetTextLength();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>a deep copy of this Element instance</returns>
        IElement Clone();

        /// <summary>
        /// Truncate
        /// Returns an IElement result consisting of this IElement and those of its children. If the cumulative text length of the 
        /// element and its children exceeds the given content length, then the text content of one child element of the result
        /// is truncated and all subsequent children of the result removed before it is returned
        /// </summary>
        /// <param name="textEndIndex">the desired content length</param>
        /// <returns></returns>
        IElement Truncate(int textEndIndex);

        /// <summary>
        /// Returns the result of Truncate(int textEndIndex) with the textToAppend string appended to the end of the
        /// text content of the final child element of the result
        /// </summary>
        /// <param name="textEndIndex">the desired content length</param>
        /// <param name="textToAppend">the string to append</param>
        /// <returns></returns>
        IElement Truncate(int textEndIndex, string textToAppend);

        /// <summary>
        /// GetHtml
        /// Gets the HTML representation of this Element instance
        /// </summary>
        /// <returns></returns>
        MvcHtmlString GetHtml();

        IMarkupValidator MarkupValidator { get; }

        /// <summary>
        /// IsValid
        /// Tests whether or not the tag context and contents context strings with which this element instance
        /// was initialised both represent valid markup
        /// </summary>
        /// <returns></returns>
        bool IsValid();

        IEnumerable<string> ValidationErrors { get; }

        IEnumerable<string> ValidationWarnings { get; }
    }

}
