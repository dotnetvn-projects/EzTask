using System;
using System.Threading.Tasks;
using EzTask.Framework.Common;
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

            model.StartDate = DateTimeUtilities.ParseFromString(model.StartDateDisplay);
            model.EndDate = DateTimeUtilities.ParseFromString(model.EndDateDisplay);
            if(model.EndDate < model.StartDate)
            {
                return BadRequest("End Date must be larger than Start Date");
            }
            var iResult = await EzTask.Phrase.Save(model);
            if (iResult.Status == ActionStatus.Ok)
            {
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
