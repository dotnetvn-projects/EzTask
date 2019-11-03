using EzTask.Framework.GlobalData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace EzTask.Web.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationAttribute :  Attribute, IAsyncAuthorizationFilter
    {
        CookiesManager _cookiesManager;

        public AuthenticationAttribute(CookiesManager cookies)
        {
            _cookiesManager = cookies;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            if (SkipAuthorization(context)) return;

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
                
                if (Context.CurrentAccount.AccountId <= 0)
                {
                    var returnUrl = context.HttpContext.Request.GetEncodedUrl();
                    context.Result = new RedirectToActionResult("Login", "Account", new { redirect = returnUrl });
                }
            });
        }

        private static bool SkipAuthorization(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                return controllerActionDescriptor.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute), true);
            }
            return false;
        }
    }
}
