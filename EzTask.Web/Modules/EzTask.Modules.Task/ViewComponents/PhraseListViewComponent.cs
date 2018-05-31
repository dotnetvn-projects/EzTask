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
            var data = await EzTask.Phrase.GetPhrases(projectId);
            return View(data);
        }
    }
}
