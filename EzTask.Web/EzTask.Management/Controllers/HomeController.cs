
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EzTask.Framework.Web.AuthorizeFilter;

namespace EzTask.Management.Controllers
{
    [TypeFilter(typeof(EzTaskAuthorizeFilter))]
    public class HomeController : EzTaskController
    {
        public HomeController(IServiceProvider serviceProvider, 
            IHttpContextAccessor httpContext)
            : base(serviceProvider, httpContext) { }

        public IActionResult Index()
        {
            PageTitle = "Home";
            return View();
        }
    }
}
