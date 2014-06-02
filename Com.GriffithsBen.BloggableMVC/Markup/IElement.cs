using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Markup {

    public interface IElement {

        int GetTextLength();

        IElement Clone();

        IElement Truncate(int textEndIndex);

        IElement Truncate(int textEndIndex, string textToAppend);

        MvcHtmlString GetHtml();

        bool IsValid();

    }

}
