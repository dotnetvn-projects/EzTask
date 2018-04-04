﻿using EzTask.Entity.Framework;
using EzTask.Framework.Values;
using EzTask.Framework.Web.HttpContext;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace EzTask.Framework.Web.HtmlHelpers
{
    public static class AccountHtmlHelper
    {
        #region Account
        public static IHtmlContent NickName(this IHtmlHelper htmlHelper)
        {
            var account = GetCurrentAccount();
            if (account != null)
                return new HtmlString(account.NickName);
            return new HtmlString(string.Empty);
        }

        public static IHtmlContent JobTitle(this IHtmlHelper htmlHelper)
        {
            var account = GetCurrentAccount();
            if (account != null)
                return new HtmlString(account.JobTitle);
            return new HtmlString(string.Empty);
        }

        public static IHtmlContent UserJoinDate(this IHtmlHelper htmlHelper)
        {
            string html = "{0}. {1}";
            var account = GetCurrentAccount();
            if (account != null)
            {
                var monthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(account.CreatedDate.Month);
                return new HtmlString(string.Format(html, monthName, account.CreatedDate.Year));
            }
            return new HtmlString(string.Empty);
        }

        #endregion

        #region private
        private static CurrentAccount GetCurrentAccount()
        {
            SessionManager sessionManager = new SessionManager();
            var currentAccount = sessionManager.GetObject<CurrentAccount>(EzTaskKey.Account);
            return currentAccount;
        }
        #endregion
    }
}
