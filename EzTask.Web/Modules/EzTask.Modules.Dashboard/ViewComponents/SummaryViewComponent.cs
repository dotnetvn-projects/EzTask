using EzTask.Business;
using EzTask.Modules.Dashboard.ViewModels;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EzTask.Modules.Dashboard.ViewComponents
{
    public class SummaryViewComponent : ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public SummaryViewComponent(EzTaskBusiness business)
        {
            EzTask = business;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<int> projectIds = await EzTask.Project.GetProjectIds(Context.CurrentAccount.AccountId);

            SummaryViewModel model = new SummaryViewModel
            {
                TotalProject = await EzTask.Project.CountProjectByUser(Context.CurrentAccount.AccountId),
                TotalNotification = await EzTask.Notification.CountNotification(Context.CurrentAccount.AccountId),
                TotalMember = await EzTask.Project.CountMemberByProductIdList(projectIds)
            };
            
            model.TotalTask = await EzTask.Task.CountTaskByProjectIdList(projectIds);

            return View(model);
        }
    }
}
