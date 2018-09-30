using EzTask.Business;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.ViewComponents
{
    public class PhraseListViewComponent : ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public PhraseListViewComponent(EzTaskBusiness business)
        {
            EzTask = business;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var data = await EzTask.Phrase.GetPhrase(projectId);
            foreach(var item in data)
            {
                var countTask = await EzTask.Task.CountByPhrase(item.Id, projectId);
                item.TotalTask = countTask;
            }
            return View(data);
        }
    }
}
