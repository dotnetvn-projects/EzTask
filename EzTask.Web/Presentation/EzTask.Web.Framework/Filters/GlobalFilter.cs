using EzTask.Web.Framework.Infrastructures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EzTask.Web.Framework.Filters
{
    /// <summary>
    /// Global filter
    /// </summary>
    public class GlobalFilter : IActionFilter
    {
        private readonly IWebHostEnvironment _env;
        public GlobalFilter(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //do not any thing
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            #if DEBUG
            //copy content file during developmen time
            // _env.RunWebBuilder(true);
            #endif
        }
    }
}
