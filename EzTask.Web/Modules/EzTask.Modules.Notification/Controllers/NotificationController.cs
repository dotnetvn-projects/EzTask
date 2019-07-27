using EzTask.Model;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzTask.Modules.Notification.Controllers
{
    [TypeFilter(typeof(ApplyLanguageAttribute))]
    [TypeFilter(typeof(AuthenticationAttribute))]
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
            ResultModel<bool> result = await EzTask.Notification.DeleteNotify(notifyId);
            return Ok(result);
        }
    }
}