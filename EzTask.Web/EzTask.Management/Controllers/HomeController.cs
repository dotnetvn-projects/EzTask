using System;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Management.Controllers
{
    public class HomeController : EzTaskController
    {
        public HomeController(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        public IActionResult Index()
        {
            PageTitle = "Home";
            return View();
        }

        [Route("not-found.html")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
