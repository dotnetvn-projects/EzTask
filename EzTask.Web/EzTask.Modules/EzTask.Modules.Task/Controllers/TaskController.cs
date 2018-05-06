using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Framework.Web.Attributes;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Task.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class TaskController : EzTaskController
    {
        public TaskController(IServiceProvider serviceProvider) : 
            base(serviceProvider)
        {
        }

        [Route("task.html")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
