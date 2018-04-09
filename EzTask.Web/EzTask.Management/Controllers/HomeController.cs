using System;
using EzTask.Framework.Web.AuthorizeFilter;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Management.Controllers
{
   [TypeFilter(typeof(EzTaskAuthorizeFilter))]
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
