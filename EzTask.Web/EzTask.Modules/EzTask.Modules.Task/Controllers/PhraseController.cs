using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Core.Infrastructures;
using EzTask.Modules.Core.Models.Phrase;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Task.Controllers
{
    public class PhraseController : EzTaskController
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

            var iResult = await EzTask.Phrase.Save(model.MapToEntity());
            if (iResult != null)
            {
                return Ok(iResult.MapToModel());
            }
            return BadRequest();
        }
    }
}
