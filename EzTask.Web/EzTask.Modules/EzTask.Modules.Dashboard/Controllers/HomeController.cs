using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EzTask.Modules.Dashboard;
using EzTask.Modules.Core.Controllers;
using EzTask.Framework.Web.Attributes;

namespace EzTask.Modules.Dashboard.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class HomeController : CoreController
    {
        public HomeController(IServiceProvider serviceProvider) : 
            base(serviceProvider)
        {                
        }

        [PageTitle("Home")]      
        public IActionResult Index()
        {
            return View();
        }
    }
}
