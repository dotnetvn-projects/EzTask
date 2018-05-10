using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Modules.Core.Controllers;
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
        public JsonResult CreateOrUpdatePhrase(PhraseModel model)
        {
            throw new Exception();
        }

        public IActionResult LoadPhraseList(int propectId)
        {
            return ViewComponent("PhraseList", new { propectId });
        }
    }
}
