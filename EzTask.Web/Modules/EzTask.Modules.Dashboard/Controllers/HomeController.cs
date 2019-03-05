using System;
using Microsoft.AspNetCore.Mvc;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;
using EzTask.Plugin.MessageService;
using EzTask.Plugin.MessageService.Data.Email;
using System.Threading.Tasks;
using EzTask.Modules.Dashboard.ViewModels;

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
    }
}
