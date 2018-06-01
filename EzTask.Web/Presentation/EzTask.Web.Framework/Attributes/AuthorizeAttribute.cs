using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
namespace EzTask.Web.Framework.Attributes
{
    class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
