using System;
using EzTask.Framework.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Web.Controllers
{
   [TypeFilter(typeof(AuthorizeFilter))]
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
            PageTitle = "Page not found";
            return View();
        }
    }
}
