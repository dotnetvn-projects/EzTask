using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EzTask.Modules.Dashboard;
using EzTask.Modules.Core.Controllers;
using EzTask.Framework.Web.Filters;

namespace EzTask.Modules.Dashboard.Controllers
{
    [TypeFilter(typeof(AuthorizeFilter))]
    public class HomeController : EzTaskController
    {
        public HomeController(IServiceProvider serviceProvider) : 
            base(serviceProvider)
        {
        }

        public IActionResult Index()
        {
            PageTitle = "Home";
            return View();
        }
    }
}
