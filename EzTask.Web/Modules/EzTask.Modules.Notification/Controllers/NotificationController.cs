using EzTask.Framework.Common;
using EzTask.Models.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.HttpContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzTask.Modules.Notification.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class NotificationController : BaseController
    {
        public NotificationController(IServiceProvider serviceProvider) 
            : base(serviceProvider) { }

        [Route("notification/list.html")]
        public async Task<IActionResult> NotificationList(short context)
        {
            var model = await EzTask.Notification.GetNotificationList(Context.CurrentAccount.AccountId,
                context.ToEnum<NotifyContext>());

            return View(model);
        }
    }
}
