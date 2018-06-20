﻿using System;
using System.Threading.Tasks;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Task.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class PhraseController : CoreController
    {
        public PhraseController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [HttpPost]
        [Route("task/phrase-modal-action.html")]
        public async Task<IActionResult> CreateOrUpdatePhrase(PhraseModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var iResult = await EzTask.Phrase.Save(model);
            if (iResult.Status == ActionStatus.Ok)
            {
                var data = iResult.Data;
                data.StartDate = DateTime.Now;
                data.EndDate = DateTime.Now.AddDays(1);
                return LoadPhraseList(model.ProjectId);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("task/phrase-list.html")]
        public IActionResult LoadPhraseList(int projectId)
        {
            return ViewComponent("PhraseList", projectId);
        }
    }
}