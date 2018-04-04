
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EzTask.Framework.Web.AuthorizeFilter;

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
            return View();
        }
    }
}
