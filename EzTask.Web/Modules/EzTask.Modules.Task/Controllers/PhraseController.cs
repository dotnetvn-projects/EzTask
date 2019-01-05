using EzTask.Framework.Common;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Task.ViewModels;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.Data;
using EzTask.Web.Framework.HttpContext;
using EzTask.Web.Framework.Infrastructures;
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
        [Route("phase/generate-phase.html")]
        public async Task<IActionResult> GeneratePhraseView(int phraseId, int projectId)
        {
            PhraseViewModel viewModel = new PhraseViewModel();
            PhraseModel phrase = await EzTask.Phrase.GetPhraseById(phraseId);
            if (phrase == null)
            {
                phrase = new PhraseModel();
            }

            viewModel.Status = phrase.Status.ToInt16<PhraseStatus>();
            viewModel.PhraseId = phraseId;
            viewModel.ProjectId = projectId;
            viewModel.IsDefault = phrase.IsDefault;
            viewModel.PhraseName = phrase.PhraseName;
            if (!viewModel.IsDefault)
            {
                viewModel.StartDate = phrase.StartDate.Value.ToString("dd/MM/yyyy");
                viewModel.EndDate = phrase.EndDate.Value.ToString("dd/MM/yyyy");
            }                
            
            viewModel.StatusList = StaticResources.BuildPhraseStatusSelectList(viewModel.Status);

            return PartialView("_CreateOrUpdatePhrase", viewModel);
        }

        [HttpPost]
        [Route("phase/phase-modal-action.html")]
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
                return await LoadPhraseListAsync(model.ProjectId);
            }

            return BadRequest("Cannot create phrase, please try again!");
        }

        [HttpGet]
        [Route("phase/phase-list.html")]
        public async Task<IActionResult> LoadPhraseListAsync(int projectId)
        {       
            var project = await EzTask.Project.GetProject(projectId);

            if (project!=null 
                && project.Owner.AccountId == Context.CurrentAccount.AccountId)
            {              
                Context.AddResponseHeader("authorized-add-phase", "authorized");
            }

            return ViewComponent("PhraseList", projectId);
        }

        [HttpPost]
        [Route("phase/delete-phase.html")]
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

        [HttpPost]
        [Route("phase/generate-addbutton.html")]
        public async Task<IActionResult> RenderAddNewPhraseButton()
        {
            var template = await Context.RenderViewToStringAsync("_ButtonAddNewPhase", ControllerContext);
            return Content(template);
        }
    }
}
