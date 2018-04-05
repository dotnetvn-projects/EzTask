using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Web.HtmlHelpers
{
    public static class EzTaskHtmlHelper
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
    }
}
