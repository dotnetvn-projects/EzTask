using EzTask.Entity.Framework;
using EzTask.Framework.Values;
using EzTask.Framework.Web.HttpContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace EzTask.Framework.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EzTaskAuthorizeFilter:Attribute, IAsyncAuthorizationFilter
    {
        SessionManager _sessionManager;
        public string ControllerName { get; set; }

        public EzTaskAuthorizeFilter()
        {
            ControllerName = string.Empty;
            _sessionManager = new SessionManager();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await Task.Factory.StartNew(() =>
            {
                var currentUser = _sessionManager.GetObject<CurrentAccount>(EzTaskKey.Account);
                if (currentUser == null 
                    || string.IsNullOrEmpty(currentUser.AccountId))
                {
                    context.Result = new RedirectToActionResult("Login","Account", new object { });
                }
            });                        
        }
    }
}
