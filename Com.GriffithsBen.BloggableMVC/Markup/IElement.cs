using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    interface IElement {

        int GetTextLength();

        IElement Truncate(int textEndIndex, string textToAppend = "...");

        MvcHtmlString GetHtml();

        Element Clone(); // TODO - does this need to be in this interface?

        bool IsValid();

    }

}
