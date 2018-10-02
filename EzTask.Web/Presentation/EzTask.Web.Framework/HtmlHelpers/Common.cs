using EzTask.Framework.Message;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EzTask.Web.Framework.HtmlHelpers
{
    public static class Common
    {
        public static IHtmlContent PageTitle(this IHtmlHelper htmlHelper)
        {          
            return new HtmlString(htmlHelper.ViewData["Title"]?.ToString());
        }

        public static IHtmlContent PrintSucessMessage(this IHtmlHelper htmlHelper)
        {
            return new HtmlString(htmlHelper.TempData["success"]?.ToString());
        }

        public static IHtmlContent PrintErrorMessage(this IHtmlHelper htmlHelper)
        {
            return new HtmlString(htmlHelper.TempData["error"]?.ToString());
        }

        public static IHtmlContent PrintPageNotFound(this IHtmlHelper htmlHelper)
        {
            return new HtmlString(AppMessage.PageNotFound);
        }

        public static IHtmlContent PrintNumberWithText(this IHtmlHelper htmlHelper,
            string text, int number)
        {
            if(number > 1)
            {
                text += "s";
            }
            return new HtmlString(number + $" {text}");
        }
    }
}
