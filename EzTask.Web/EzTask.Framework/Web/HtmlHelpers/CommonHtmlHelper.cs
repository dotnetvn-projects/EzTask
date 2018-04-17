using EzTask.Framework.Message;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EzTask.Framework.Web.HtmlHelpers
{
    public static class CommonHtmlHelper
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
    }
}
