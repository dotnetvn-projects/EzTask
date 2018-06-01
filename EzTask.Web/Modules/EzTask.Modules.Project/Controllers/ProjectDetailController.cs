using System;
using System.Threading.Tasks;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Project.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class ProjectDetailController : CoreController
    {
        public ProjectDetailController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("project/{code}.html")]
        public async Task<IActionResult> Index(string code)
        {
            var data = await EzTask.Project.GetProjectDetail(code);
            return View(data);
        }

        #region Non Action
        #endregion
    }
}
