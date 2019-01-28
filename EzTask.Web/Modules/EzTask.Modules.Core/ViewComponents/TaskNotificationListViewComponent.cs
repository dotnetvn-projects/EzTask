using EzTask.Business;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzTask.Modules.Core.ViewComponents
{
    public class TaskNotificationListViewComponent : ViewComponent
    {
        private readonly EzTaskBusiness _ezTask;
        public TaskNotificationListViewComponent(EzTaskBusiness eztask)
        {
            _ezTask = eztask;
        }

        public async Task<IViewComponentResult> InvokeAsync(int assigneeId)
        {
            var result = await _ezTask.Task.GetTasksByAssigneeId(assigneeId, 999);
            return View(result.Data);
        }
    }
}
