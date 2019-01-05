using EzTask.Framework.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;
using EzTask.Web.Framework.HttpContext;

namespace EzTask.Web.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenAttribute :  Attribute, IAsyncAuthorizationFilter
    {
        CookiesManager _cookiesManager;
        public string ControllerName { get; set; }

        public AuthenAttribute(CookiesManager cookies)
        {
            ControllerName = string.Empty;
            _cookiesManager = cookies;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await Task.Factory.StartNew(() =>
            {
                if (Context.CurrentAccount.AccountId <= 0)
                {
                    var cookieUser = _cookiesManager.GetObject<CurrentAccount>(SessionKey.EzTaskAuthen);
                    if (cookieUser != null)
                    {
                        Context.CurrentAccount.Set(cookieUser);
                        Context.SetLanguageLocalization(cookieUser.Language);
                    }
                }
                if (Context.CurrentAccount.AccountId <=0)
                {
                    var returnUrl = context.HttpContext.Request.GetEncodedUrl();
                    context.Result = new RedirectToActionResult("Login", "Account", new { redirect = returnUrl });
                }
            });
        }
    }
}
