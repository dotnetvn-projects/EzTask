﻿using EzTask.Entity.Framework;
using EzTask.Framework.FrameworkObjects;
using EzTask.Framework.Web.HttpContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;

namespace EzTask.Framework.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenAttribute :  Attribute, IAsyncAuthorizationFilter
    {
        SessionManager _sessionManager;
        CookiesManager _cookiesManager;
        public string ControllerName { get; set; }

        public AuthenAttribute(SessionManager session, CookiesManager cookies)
        {
            ControllerName = string.Empty;
            _sessionManager = session;
            _cookiesManager = cookies;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await Task.Factory.StartNew(() =>
            {
                var currentUser = _sessionManager.GetObject<CurrentAccount>(AppKey.Account);
                if (currentUser == null)
                {
                    currentUser = _cookiesManager.GetObject<CurrentAccount>(AppKey.EzTaskAuthen);
                    if (currentUser != null)
                    {
                        _sessionManager.SetObject(AppKey.Account, currentUser);
                    }
                }
                if (currentUser == null
                    || string.IsNullOrEmpty(currentUser.AccountId))
                {
                    var returnUrl = context.HttpContext.Request.GetEncodedUrl();
                    context.Result = new RedirectToActionResult("Login", "Account", new { redirect = returnUrl });
                }
            });
        }
    }
}
