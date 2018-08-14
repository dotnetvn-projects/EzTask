using EzTask.Business;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.ViewComponents
{
    public class HistoryListViewComponent :ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public HistoryListViewComponent(EzTaskBusiness business)
        {
            EzTask = business;
        }

        public async Task<IViewComponentResult> InvokeAsync(int taskId)
        {
            var data = await EzTask.Attachment.GetAttachments(taskId);
            return View(data);
        }
    }
}
