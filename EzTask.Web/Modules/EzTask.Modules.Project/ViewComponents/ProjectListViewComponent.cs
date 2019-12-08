//using EzTask.Modules.Core.ViewComponents;
using EzTask.Business;
using EzTask.Framework.Common;
using EzTask.Modules.Project.ViewModels;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Project.ViewComponents
{
    public class ProjectListViewComponent : ViewComponent
    {
        private readonly EzTaskBusiness _ezTask;
        public ProjectListViewComponent(EzTaskBusiness eztask)
        {
            _ezTask = eztask;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await GetProjectList();
            return View(data);
        }

        private async Task<IList<List<ProjectViewModel>>> GetProjectList()
        {
            IList<List<ProjectViewModel>> models = new List<List<ProjectViewModel>>();
            var data = await _ezTask.Project.GetProjects(Context.CurrentAccount.AccountId);
            if (data == null)
            {
                return models;
            }

            var viewModels = new List<ProjectViewModel>();

            foreach (var proj in data)
            {
                ProjectViewModel vm = new ProjectViewModel()
                {
                    Project = proj,
                    TotalTask = await _ezTask.Task.CountTaskByProjectId(proj.ProjectId)
                };
                var members = await _ezTask.Project.GetAccountList(proj.ProjectId);
                foreach (var mem in members)
                {
                    mem.TotalTask = await _ezTask.Task.CountTaskByMember(mem.AccountId, proj.ProjectId);
                    vm.Members.Add(mem);
                }
                viewModels.Add(vm);
            }

            models = viewModels.SplitList(3).ToList();

            return models;
        }
    }
}
