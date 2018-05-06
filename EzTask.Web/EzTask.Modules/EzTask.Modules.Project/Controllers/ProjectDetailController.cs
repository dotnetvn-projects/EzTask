using System;
using System.Threading.Tasks;
using EzTask.Framework.Web.Attributes;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using EzTask.Modules.Core.Infrastructures;

namespace EzTask.Modules.Project.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class ProjectDetailController : EzTaskController
    {
        public ProjectDetailController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("project/{code}.html")]
        public async Task<IActionResult> Index(string code)
        {
            var data = await EzTask.Project.GetProjectDetail(code);
            var model = data.MapToModel();
            return View(model);
        }

        #region Non Action
        #endregion
    }
}
