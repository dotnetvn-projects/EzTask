using EzTask.Framework.Common;
using EzTask.Models.Enum;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.HttpContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzTask.Modules.Core.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class NotificationController : BaseController
    {
        public NotificationController(IServiceProvider serviceProvider) 
            : base(serviceProvider) { }

        [HttpPost]
        [Route("notify/get-new")]
        public async Task<IActionResult> _NewNotificationList(short context)
        {
            var model = await EzTask.Notification.NewNotificationList(Context.CurrentAccount.AccountId,
                context.ToEnum<NotifyContext>());

            return PartialView("_NewNotificationList", model);
        }

        [HttpPost]
        [Route("notify/get-all")]
        public async Task<IActionResult> _NotificationList(short context)
        {
            var model = await EzTask.Notification.GetNotificationList(Context.CurrentAccount.AccountId,
                context.ToEnum<NotifyContext>());

            return PartialView("_NotificationList", model);
        }
    }
}
