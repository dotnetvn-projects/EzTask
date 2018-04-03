using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Framework.Web.AuthorizeFilter;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Management.Controllers
{
    [TypeFilter(typeof(EzTaskAuthorizeFilter))]
    public class ProjectController : EzTaskController
    {
        public ProjectController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("projects.html")]
        public IActionResult Index()
        {
            PageTitle = "Projects";
            return View();
        }
    }
}