using EzTask.Business;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzTask.Modules.Dashboard.ViewComponents
{
    public class CurrentTaskListViewComponent : ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public CurrentTaskListViewComponent(EzTaskBusiness business)
        {
            EzTask = business;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await EzTask.Task.GetTasksByMemberId(Context.CurrentAccount.AccountId);
            return View(result.Data);
        }
    }
}
