﻿using EzTask.Business;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.ViewComponents
{
    public class HistoryListViewComponent : ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public HistoryListViewComponent(EzTaskBusiness business)
        {
            EzTask = business;
        }

        public async Task<IViewComponentResult> InvokeAsync(int taskId, int accountId)
        {
            var data = await EzTask.Task.GetHistoryList(taskId, accountId);
            return View(data);
        }
    }
}
