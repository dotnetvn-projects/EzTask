using EzTask.Business;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var data = await EzTask.Task.GetTasks(projectId, 1, 1000);
            return View(data);
        }
    }
}
