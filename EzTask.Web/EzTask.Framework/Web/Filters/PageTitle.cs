using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzTask.Framework.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PageTitle : Attribute, IAsyncActionFilter
    {
        private string _title;
        public PageTitle(string pageTitle)
        {
            _title = pageTitle;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context,
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
