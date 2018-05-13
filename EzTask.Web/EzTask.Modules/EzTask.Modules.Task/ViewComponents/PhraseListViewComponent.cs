﻿using EzTask.Business;
using EzTask.Modules.Core.Infrastructures;
//using EzTask.Modules.Core.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Tasks.ViewComponents
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
            return View(data.MapToModels());
        }
    }
}
