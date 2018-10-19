using EzTask.Framework.Data;
using EzTask.Web.Framework.HttpContext;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;

namespace EzTask.Web.Framework.HtmlHelpers
{
    public static class Account
    {
        #region Account
        public static IHtmlContent DisplayName(this IHtmlHelper htmlHelper)
        {
            var displayName = Context.CurrentAccount.DisplayName;
            if (!string.IsNullOrEmpty(displayName))
                return new HtmlString(displayName);
            return new HtmlString(string.Empty);
        }

        public static IHtmlContent JobTitle(this IHtmlHelper htmlHelper)
        {
            var jobTitle = Context.CurrentAccount.JobTitle;
            if (!string.IsNullOrEmpty(jobTitle))
                return new HtmlString(jobTitle);
            return new HtmlString(string.Empty);
        }

        public static IHtmlContent UserJoinDate(this IHtmlHelper htmlHelper)
        {
            string html = "{0}. {1}";
            var createdDate = Context.CurrentAccount.CreatedDate;
            if (createdDate != DateTime.MinValue)
            {
                var monthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(createdDate.Month);
                return new HtmlString(string.Format(html, monthName, createdDate.Year));
            }
            return new HtmlString(string.Empty);
        }

        #endregion
    }
}
