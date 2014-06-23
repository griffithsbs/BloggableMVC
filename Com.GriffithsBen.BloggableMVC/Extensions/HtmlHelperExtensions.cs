using Com.GriffithsBen.BloggableMVC.Abstract;
using Com.GriffithsBen.BloggableMVC.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Com.GriffithsBen.BloggableMVC.Extensions {

    public static class HtmlHelperExtensions {

        private class MarkupableProxy : IMarkupable {
            public string Content { get; set; }
        }

        public static MvcHtmlString BlogContentFor<TModel, TResult>(this HtmlHelper helper, 
                                                                    Expression<Func<TModel, TResult>> expression,
                                                                    object htmlAttributes = null) {

            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (expression == null) {
                throw new ArgumentException("expression is expected to be a MemberExpression");
            }

            UnaryExpression objectMember = Expression.Convert(memberExpression, typeof(object));

            Expression<Func<object>> getterLambda = Expression.Lambda<Func<object>>(objectMember);

            Func<object> getter = getterLambda.Compile();

            string content = getter() as string;

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

            // TODO - root element
            TagBuilder tag = new TagBuilder("p");

            if (htmlAttributes != null && htmlAttributes is Dictionary<string, string>) {
                tag.MergeAttributes((Dictionary<string, string>)htmlAttributes);
            }

            tag.InnerHtml = markupable.ContentHtml.ToString();

            return new MvcHtmlString(tag.ToString());

        }

    }

}
