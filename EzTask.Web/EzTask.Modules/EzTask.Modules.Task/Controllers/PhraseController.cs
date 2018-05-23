using System;
using System.Threading.Tasks;
using EzTask.Models;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Task.Controllers
{
    public class PhraseController : CoreController
    {
        public PhraseController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpPost]
        [Route("task/phrase-modal-action.html")]
        public async Task<IActionResult> CreateOrUpdatePhrase(PhraseModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var iResult = await EzTask.Phrase.Save(model);
            if (iResult != null)
            {
                return Json(iResult);
            }
            return BadRequest();
        }
    }
}
