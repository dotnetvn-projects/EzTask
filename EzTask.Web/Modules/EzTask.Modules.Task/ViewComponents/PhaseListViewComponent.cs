using EzTask.Business;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.ViewComponents
{
    public class PhaseListViewComponent : ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public PhaseListViewComponent(EzTaskBusiness business)
        {
            EzTask = business;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var data = await EzTask.Phase.GetPhases(projectId);
            foreach(var item in data)
            {
                var countTask = await EzTask.Task.CountByPhase(item.Id, projectId);
                item.TotalTask = countTask;
            }

            return View(data);
        }
    }
}
