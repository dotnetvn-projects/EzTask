using System;
using System.Threading.Tasks;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Project.ViewModels;
using EzTask.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Project.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class ProjectDetailController : BaseController
    {
        public ProjectDetailController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("project/{code}.html")]
        public async Task<IActionResult> Index(string code)
        {
            var model = await EzTask.Project.GetProjectDetail(code);
            var vm = new ProjectViewModel();
            vm.Project = model;
            vm.TotalTask = await EzTask.Task.CountByProject(model.ProjectId);
            vm.TotalPhrase = await EzTask.Phrase.CountByProject(model.ProjectId);
            vm.TotalMember = await EzTask.Project.CountMember(model.ProjectId);

            return View(vm);
        }

        #region Non Action
        #endregion
    }
}
