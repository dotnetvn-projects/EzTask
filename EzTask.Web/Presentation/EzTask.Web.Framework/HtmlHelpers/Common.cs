using EzTask.Framework.Message;
using EzTask.Web.Framework.HttpContext;
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

        /// <summary>
        /// Get message title from resources
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlContent GetMessageTitleString(this IHtmlHelper htmlHelper, string key)
        {
            var lang = Context.GetLanguageLocalization();
            var content = lang.GetMessageTitleLang(key);
            return new HtmlString(content);
        }

        /// <summary>
        /// Get home page string from resources
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlContent GetDashboardPageString(this IHtmlHelper htmlHelper, string key)
        {
            var lang = Context.GetLanguageLocalization();
            var content = lang.GetDashboardPageLang(key);
            return new HtmlString(content);
        }

        /// <summary>
        /// Get project page string from resources
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlContent GetProjectPageString(this IHtmlHelper htmlHelper, string key)
        {
            var lang = Context.GetLanguageLocalization();
            var content = lang.GetProjectPageLang(key);
            return new HtmlString(content);
        }

        /// <summary>
        /// Get common string from resources
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlContent GetCommonString(this IHtmlHelper htmlHelper, string key)
        {
            var lang = Context.GetLanguageLocalization();
            var content = lang.GetCommonMessageLang(key);
            return new HtmlString(content);
        }

        /// <summary>
        /// Get error string from resources
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlContent GetErrorString(this IHtmlHelper htmlHelper, string key)
        {
            var lang = Context.GetLanguageLocalization();
            var content = lang.GetErrorMessageLang(key);
            return new HtmlString(content);
        }

        /// <summary>
        /// Get success string from resources
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlContent GetSuccessString(this IHtmlHelper htmlHelper, string key)
        {
            var lang = Context.GetLanguageLocalization();
            var content = lang.GetSuccessMessageLang(key);
            return new HtmlString(content);
        }
    }
}
