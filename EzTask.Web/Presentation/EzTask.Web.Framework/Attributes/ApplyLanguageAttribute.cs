using EzTask.Framework.GlobalData;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace EzTask.Web.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApplyLanguageAttribute : ActionFilterAttribute
    {
        private readonly CookiesManager _cookiesManager;

        public ApplyLanguageAttribute(CookiesManager cookiesManager)
        {
            _cookiesManager = cookiesManager;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (Context.CurrentAccount.AccountId <= 0)
            {
                var cookieUser = _cookiesManager.GetObject<CurrentAccount>(SessionKey.EzTaskAuthen);
                if (cookieUser != null)
                {
                    Context.SetLanguageLocalization(cookieUser.Language);
                }
            }

            if (Context.CurrentAccount.AccountId <= 0)
            {
                Context.SetLanguageLocalization("");
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
