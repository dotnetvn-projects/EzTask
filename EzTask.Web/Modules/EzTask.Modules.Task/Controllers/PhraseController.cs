using EzTask.Framework.Common;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Task.Models;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class PhraseController : BaseController
    {
        public PhraseController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [HttpPost]
        [Route("task/generate-phrase.html")]
        public async Task<IActionResult> GeneratePhraseView(int phraseId)
        {
            PhraseViewModel viewModel = new PhraseViewModel();
            PhraseModel phrase = await EzTask.Phrase.GetPhraseById(phraseId);
            if (phrase == null)
            {
                phrase = new PhraseModel();
            }
            else
            {
                viewModel.Status = phrase.Status.ToInt16<PhraseStatus>();
                viewModel.PhraseId = phrase.Id;
                viewModel.ProjectId = phrase.ProjectId;
                viewModel.IsDefault = phrase.IsDefault;
                viewModel.PhraseName = phrase.PhraseName;
                if (!viewModel.IsDefault)
                {
                    viewModel.StartDate = phrase.StartDate.Value.ToString("dd/MM/yyyy");
                    viewModel.EndDate = phrase.EndDate.Value.ToString("dd/MM/yyyy");
                }                
            }

            viewModel.StatusList = StaticResources.BuildPhraseStatusSelectList(viewModel.Status);

            return PartialView("_CreateOrUpdatePhrase", viewModel);
        }

        [HttpPost]
        [Route("task/phrase-modal-action.html")]
        public async Task<IActionResult> CreateOrUpdatePhrase(PhraseViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            PhraseModel model = new PhraseModel()
            {
                StartDate = DateTimeUtilities.ParseFromString(viewmodel.StartDate),
                EndDate = DateTimeUtilities.ParseFromString(viewmodel.EndDate),
                Id = viewmodel.PhraseId,
                PhraseName = viewmodel.PhraseName,
                ProjectId = viewmodel.ProjectId,
                IsDefault = viewmodel.IsDefault,
                Status = viewmodel.Status.ToEnum<PhraseStatus>()
            };


            if (model.EndDate < model.StartDate)
            {
                return BadRequest("End Date must be larger than Start Date");
            }

            ResultModel<PhraseModel> iResult = await EzTask.Phrase.Save(model);
            if (iResult.Status == ActionStatus.Ok)
            {
                return LoadPhraseList(model.ProjectId);
            }

            return BadRequest("Cannot create phrase, please try again!");
        }

        [HttpGet]
        [Route("task/phrase-list.html")]
        public IActionResult LoadPhraseList(int projectId)
        {
            return ViewComponent("PhraseList", projectId);
        }

        [HttpPost]
        [Route("task/delete-phrase.html")]
        public async Task<IActionResult> RemovePhrase(int phraseId)
        {
            PhraseModel phrase = await EzTask.Phrase.GetPhraseById(phraseId);

            if (phrase != null)
            {
                if (phrase.IsDefault)
                {
                    return BadRequest("You cannot delete \"Open Features\"!.");
                }
                ResultModel<bool> deleteTaskResult = await EzTask.Task.DeleteTask(phrase.ProjectId, phraseId);

                if (deleteTaskResult.Data)
                {
                    ResultModel<PhraseModel> iResult = await EzTask.Phrase.Delele(phrase);

                    if (iResult.Status == ActionStatus.Ok)
                    {
                        return Ok();
                    }
                }
            }
            else
            {
                return BadRequest("Phrase doesn't exist!.");
            }

            return BadRequest("Error, cannot process your request!.");
        }

    }
}
