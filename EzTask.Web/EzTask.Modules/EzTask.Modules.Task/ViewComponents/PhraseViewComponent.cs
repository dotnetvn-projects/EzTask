using EzTask.Business;
using EzTask.Modules.Core.Infrastructures;
//using EzTask.Modules.Core.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Tasks.ViewComponents
{
    public class PhraseViewComponent : ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public PhraseViewComponent(EzTaskBusiness serviceProvider)
        {
            EzTask = serviceProvider;
        }


        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var data = await EzTask.Phrase.GetPhrases(projectId);
            return View(data.MapToModels());
        }
    }
}
