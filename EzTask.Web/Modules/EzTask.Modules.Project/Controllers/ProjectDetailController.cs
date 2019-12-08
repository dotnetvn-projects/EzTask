using EzTask.Model.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Project.ViewModels;
using EzTask.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Project.Controllers
{
    [TypeFilter(typeof(ApplyLanguageAttribute))]
    [TypeFilter(typeof(AuthenticationAttribute))]
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
            vm.TotalTask = await EzTask.Task.CountTaskByProjectId(model.ProjectId);
            vm.TotalPhase = await EzTask.Phase.CountByProject(model.ProjectId);
            vm.TotalMember = await EzTask.Project.CountMemberByProjectId(model.ProjectId);

            var phases = await EzTask.Phase.GetPhases(model.ProjectId);
            if (phases.Any())
            {
                vm.TotalOpenPhase = phases.Count(c => c.Status == PhaseStatus.Open);
                vm.TotalClosedPhase = phases.Count(c => c.Status == PhaseStatus.Closed);
                vm.TotalFailedPhase = phases.Count(c => c.Status == PhaseStatus.Failed);

                vm.PercentOpenPhase = (int)(vm.TotalOpenPhase / (float)vm.TotalPhase * 100);
                vm.PercentClosedPhase = (int)(vm.TotalClosedPhase / (float)vm.TotalPhase * 100);
                vm.PercentFailedPhase = (int)(vm.TotalFailedPhase / (float)vm.TotalPhase * 100);

                foreach (var phase in phases)
                {
                    var tasks = await EzTask.Task.GetTasks(model.ProjectId, phase.Id);
                    if (tasks.Any())
                        vm.TaskList.Add(tasks.ToList());
                }
            }

            return View(vm);
        }

        #region Non Action


        #endregion
    }
}
