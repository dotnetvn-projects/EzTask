using EzTask.Framework.Data;
using EzTask.Web.Framework.HttpContext;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

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

        public static IHtmlContent PrintNumberWithText(this IHtmlHelper htmlHelper,
            string text, int number)
        {
            if(number > 1)
            {
                text += "s";
            }
            return new HtmlString(number + $" {text}");
        }

        /// <summary>
        /// Get string resources
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlContent GetStringResource(this IHtmlHelper htmlHelper, string key, StringResourceType resourceType)
        {
            var content = Context.GetStringResource(key, resourceType);
            return new HtmlString(content);
        }

        public static IHtmlContent DisplayPlanText(this IHtmlHelper htmlHelper, string text)
        {
            return new HtmlString(Regex.Replace(text, "<.*?>", string.Empty));
        }
    }
}
