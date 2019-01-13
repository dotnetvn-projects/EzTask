﻿using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.HttpContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzTask.Modules.Notification.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class NotificationController : BaseController
    {
        public NotificationController(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        [Route("notification/list.html")]
        public IActionResult NotificationList()
        {
            return View();
        }

        [Route("notification/load-list.html")]
        public IActionResult LoadNotificationList()
        {
            return ViewComponent("NotificationList");
        }

        [HttpPost]
        [Route("notification/delete.html")]
        public async Task<IActionResult> DeleteNotifyItem(int notifyId)
        {
           var result = await EzTask.Notification.DeleteNotify(notifyId);
           return Ok(result);
        }
    }
}
