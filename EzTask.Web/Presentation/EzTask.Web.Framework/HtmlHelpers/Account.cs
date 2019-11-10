using EzTask.Web.Framework.WebContext;
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
            {
                return new HtmlString(displayName);
            }
            else
            {
                return new HtmlString(string.Empty);
            }
        }

        public static IHtmlContent JobTitle(this IHtmlHelper htmlHelper)
        {
            var jobTitle = Context.CurrentAccount.JobTitle;

            if (!string.IsNullOrEmpty(jobTitle))
            {
                return new HtmlString(jobTitle);
            }
            else
            {
                return new HtmlString(string.Empty);
            }
        }

        public static IHtmlContent UserJoinDate(this IHtmlHelper htmlHelper)
        {
            var createdDate = Context.CurrentAccount.CreatedDate;

            if (createdDate != DateTime.MinValue)
            {
                var monthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(createdDate.Month);
                return new HtmlString($"{monthName}. {createdDate.Year}");
            }
            else
            {
                return new HtmlString(string.Empty);
            }
        }

        #endregion
    }
}
