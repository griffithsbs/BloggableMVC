using Com.GriffithsBen.BloggableMVC.Abstract;
using Com.GriffithsBen.BloggableMVC.Concrete;
using Com.GriffithsBen.BloggableMVC.Configuration;
using Com.GriffithsBen.BloggableMVC.Markup;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Extensions {

    public static class HtmlHelperExtensions {

        private class MarkupableProxy : IMarkupable {
            public string Content { get; set; }
        }

        public static MvcHtmlString BlogContentFor<TModel, TResult>(this HtmlHelper<TModel> helper, 
                                                                    Expression<Func<TModel, TResult>> expression,
                                                                    object htmlAttributes = null) {

            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (expression == null) {
                throw new ArgumentException("expression is expected to be a MemberExpression");
            }

            Expression<Func<TModel, string>> getterLambda = Expression.Lambda<Func<TModel, string>>(memberExpression, expression.Parameters);
            
            string content = getterLambda.Compile()(helper.ViewData.Model);

            if (content == null) {
                throw new ArgumentException("The member expression is expected to access a property of type string");
            }

            return helper.BlogContent(content, htmlAttributes);
        }

        public static MvcHtmlString BlogContent(this HtmlHelper helper, string content, object htmlAttributes = null) {

            if (content == null) {
                throw new ArgumentNullException("content");
            }

            Markupable markupable = new Markupable(
                new MarkupableProxy() {
                    Content = content
                }
            );

            ProxyTagDelimiter proxyTagDelimiter = MarkupConfiguration.ProxyTagDelimiter;

            TagBuilder tag = new TagBuilder(MarkupConfiguration.RootElementTagContext
                                                               .TrimStart(proxyTagDelimiter.GetOpeningCharacter())
                                                               .TrimEnd(proxyTagDelimiter.GetClosingCharacter()));

            if (htmlAttributes != null) {
                tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }

            // hack. we have doubled up on the root element here for the purposes of adding the html attributes,
            // so we remove the innermost of the 'two roots'
            MarkupElement rootElement = MarkupConfiguration.GetMarkupElementForMatch(MarkupConfiguration.RootElementTagContext);
            if(rootElement == null) {
                throw new InvalidOperationException(string.Format("No matching markup element found for configrued root element tag context, \"{0}\"", MarkupConfiguration.RootElementTagContext));
            }
            int innerRootOpeningTagLength = rootElement.HtmlElement.Length + 2;
            int innerRootClosingTagLength = innerRootOpeningTagLength + 1;
            int contentLength = markupable.ContentHtml.ToString().Length;
            string contentHtml = markupable.ContentHtml.ToString();

            tag.InnerHtml = contentHtml.Remove(0, innerRootOpeningTagLength)
                                       .Remove(contentLength - innerRootOpeningTagLength - innerRootClosingTagLength, 
                                               innerRootClosingTagLength);

            return new MvcHtmlString(tag.ToString());

        }

    }

}
