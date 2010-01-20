using System;
using System.Linq.Expressions;
using FubuMVC.Core.View;
using FubuMVC.UI.Forms;
using FubuMVC.UI.Tags;
using HtmlTags;

namespace FubuMVC.UI
{
    public static class FubuPageExtensions
    {
        public static TagGenerator<T> Tags<T>(this IFubuPage<T> page) where T : class
        {
            return page.Get<TagGenerator<T>>();
        }

        public static HtmlTag InputFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression) where T : class
        {
            return page.Tags().InputFor(expression);
        }

        public static HtmlTag LabelFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression) where T : class
        {
            return page.Tags().LabelFor(expression);
        }

        public static HtmlTag DisplayFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression) where T : class
        {
            return page.Tags().DisplayFor(expression);
        }

        public static FormLineExpression<T> LabeledDisplayFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression) where T : class
        {
            return new FormLineExpression<T>(page.Tags(), page.Get<ILabelAndFieldLayout>(), expression);
        }

        public static FormLineExpression<T> LabeledInputFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression) where T : class
        {
            return new FormLineExpression<T>(page.Tags(), page.Get<ILabelAndFieldLayout>(), expression).EditableIf(true);
        }
    }
}