﻿using EzTask.Framework.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Interfaces;
using EzTask.Web.Framework.Infrastructures;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzTask.Web.Framework.HttpContext
{
    public static class Context
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private static SessionManager _sessionManager;
        private static CookiesManager _cookiesManager;
        private static IWebHostEnvironment _webHostEnvironment;

        public static void Configure(this IApplicationBuilder applicationBuilder,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment,
            SessionManager sessionManager, CookiesManager cookiesManager)
        {
            _httpContextAccessor = httpContextAccessor;
            CurrentAccount = new AccountContext(sessionManager);
            _cookiesManager = cookiesManager;
            _sessionManager = sessionManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public static string GetRemoteIP
        {
            get { return Current.Connection.RemoteIpAddress.ToString(); }
        }

        public static string GetUserAgent
        {
            get { return Current.Request.Headers["User-Agent"].ToString(); }
        }

        public static string GetScheme
        {
            get { return Current.Request.Scheme; }
        }

        public static string GetHost
        {
            get { return Current.Request.Host.ToUriComponent(); }
        }

        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get => _httpContextAccessor.HttpContext;
        }

        public static AccountContext CurrentAccount { get; private set; }

        /// <summary>
        /// Set language
        /// </summary>
        /// <param name="lang"></param>
        public static void SetLanguageLocalization(string lang)
        {
            if (string.IsNullOrEmpty(lang))
            {
                lang = "lang-en.xml";
            }

            var langPath = _webHostEnvironment.GetRootContentUrl() + "/resources/languages/" + lang;

            var mapper = new LocalizationMapper(langPath);

            _sessionManager.SetObject(SessionKey.LanguageSetting, mapper);           
        }

        /// <summary>
        /// Get language
        /// </summary>
        /// <returns></returns>
        public static LocalizationMapper GetLanguageLocalization()
        {
            var data = _sessionManager.GetObject<LocalizationMapper>(SessionKey.LanguageSetting);
            return data;
        }

        /// <summary>
        /// Suspend session
        /// </summary>
        public static void SuspendSession(SessionKey key)
        {
            _sessionManager.Remove(key);
        }

        /// <summary>
        /// Suspend cookie
        /// </summary>
        public static void SuspendCookie(SessionKey key)
        {
            _cookiesManager.Remove(key);
        }

        /// <summary>
        /// Remember login
        /// </summary>
        public static void RememberLogin(CurrentAccount currentAccount)
        {
            _cookiesManager.SetObject(SessionKey.EzTaskAuthen, currentAccount, 3000);
        }

        /// <summary>
        /// Add response header
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static void AddResponseHeader(string key, object data)
        {
            Current.Response.Headers.Add(key, data.ToString());
        }

        /// <summary>
        /// Render view to html
        /// </summary>
        /// <param name="viewname"></param>
        /// <param name="controllerContext"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<string> RenderViewToStringAsync(string viewname, ControllerContext controllerContext, 
            object model = null)
        {
            var viewRender = Current.RequestServices.InvokeComponents<ViewRender>();
            var html = await viewRender.RenderToStringAsync(viewname, controllerContext, model);
            return html;
        }
    }
}
