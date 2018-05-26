using System;
using System.Threading.Tasks;
using EzTask.Entity.Framework;
using EzTask.Models;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Task.Controllers
{
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
                var data = (PhraseModel)iResult.Value;
                data.StartDate = DateTime.Now;
                data.EndDate = DateTime.Now.AddDays(1);
                return Json(data);
            }

            return BadRequest();
        }
    }
}
