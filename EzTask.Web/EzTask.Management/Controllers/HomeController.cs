
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EzTask.Management.Controllers
{
    public class HomeController : EzTaskController
    {
        public HomeController(IServiceProvider serviceProvider, IHttpContextAccessor httpContext)
            : base(serviceProvider, httpContext) { }

        public IActionResult Index()
        {
            PageTitle = "Home";
            return View();
        }
    }
}
