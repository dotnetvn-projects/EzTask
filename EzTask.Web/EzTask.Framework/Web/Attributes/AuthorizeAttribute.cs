using EzTask.Entity.Framework;
using EzTask.Framework.Values;
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
    class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
