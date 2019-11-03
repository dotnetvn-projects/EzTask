using EzTask.Business;
using EzTask.Modules.Task.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.ViewComponents
{
    public class TaskListViewComponent : ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public TaskListViewComponent(EzTaskBusiness business)
        {
            EzTask = business;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId, int phaseId)
        {
            TaskViewModel viewModel = new TaskViewModel();

            var data = await EzTask.Task.GetTasks(projectId, phaseId);
            viewModel.TaskList = data;

            if(viewModel.TaskList.Any())
            {
                viewModel.Phase = viewModel.TaskList.First().Phase;
            }
            else
            {
                viewModel.Phase = await EzTask.Phase.GetPhaseById(phaseId);
            }
            viewModel.Project = await EzTask.Project.GetProject(projectId);

            return View(viewModel);
        }
    }
}
