using System;
using Microsoft.AspNetCore.Mvc;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;

namespace EzTask.Modules.Dashboard.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class HomeController : BaseController
    {
        public HomeController(IServiceProvider serviceProvider) : 
            base(serviceProvider)
        {
        }
    
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// load to do list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("dashboard/todo-list.html")]
        public IActionResult GetTaskList(int currentPage)
        {
            return ViewComponent("TodoList", new { currentPage });
        }
    }
}
