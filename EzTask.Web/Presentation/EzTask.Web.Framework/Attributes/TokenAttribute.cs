using Microsoft.AspNetCore.Mvc.Filters;

namespace EzTask.Web.Framework.Attributes
{
    public class TokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
           
        }
    }
}
