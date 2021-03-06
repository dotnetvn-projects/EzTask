﻿using EzTask.Business;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzTask.Modules.Notification.ViewComponents
{
    public class NotificationListViewComponent : ViewComponent
    {
        private readonly EzTaskBusiness _ezTask;

        public NotificationListViewComponent(EzTaskBusiness eztask)
        {
            _ezTask = eztask;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _ezTask.Notification.GetNotificationList(Context.CurrentAccount.AccountId);
            await _ezTask.Notification.UpdateNotifyStatus(Context.CurrentAccount.AccountId);
            return View(model.Data);
        }
    }
}