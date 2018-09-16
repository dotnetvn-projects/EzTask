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

        [PageTitle("Home")]      
        public IActionResult Index()
        {
            return View();
        }
    }
}
