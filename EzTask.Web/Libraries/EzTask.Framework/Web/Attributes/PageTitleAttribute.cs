using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

using System.Threading.Tasks;

namespace EzTask.Framework.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PageTitleAttribute : ActionFilterAttribute
    {
        private string _title;
        public PageTitleAttribute(string pageTitle)
        {
            _title = pageTitle;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            controller.ViewData["Title"] = "EzTask - " + _title;
            await next();
        }
        
        public static void CombineWith(Controller controller, string title)
        {
            controller.ViewData["Title"] += title;
        }
    }
}
