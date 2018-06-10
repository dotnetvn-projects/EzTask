using EzTask.Business;
using EzTask.Modules.Task.Models;
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

        public async Task<IViewComponentResult> InvokeAsync(int projectId, int phraseId)
        {
            TaskViewModel viewModel = new TaskViewModel();

            var data = await EzTask.Task.GetTasks(projectId, phraseId);
            viewModel.TaskList = data;

            if(viewModel.TaskList.Any())
            {
                viewModel.Phrase = viewModel.TaskList.First().Phrase;
            }
            else
            {
                viewModel.Phrase = await EzTask.Phrase.GetPhraseById(phraseId);
            }

            return View(viewModel);
        }
    }
}
