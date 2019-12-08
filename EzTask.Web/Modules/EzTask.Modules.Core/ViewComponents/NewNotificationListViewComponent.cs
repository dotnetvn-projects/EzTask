using EzTask.Business;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzTask.Modules.Core.ViewComponents
{
    public class NewNotificationListViewComponent : ViewComponent
    {
        private readonly EzTaskBusiness _ezTask;
        public NewNotificationListViewComponent(EzTaskBusiness eztask)
        {
            _ezTask = eztask;
        }

        public async Task<IViewComponentResult> InvokeAsync(int accountId)
        {
            var result = await _ezTask.Notification.NewNotificationList(accountId);
            return View(result.Data);
        }
    }
}
