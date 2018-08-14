using EzTask.Business;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.ViewComponents
{
    public class AttachmentsViewComponent : ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public AttachmentsViewComponent(EzTaskBusiness business)
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
