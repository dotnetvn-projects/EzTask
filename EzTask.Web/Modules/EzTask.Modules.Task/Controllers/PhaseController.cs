using EzTask.Framework.Common;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Task.ViewModels;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.Data;
using EzTask.Web.Framework.HttpContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class PhaseController : BaseController
    {
        public PhaseController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [HttpPost]
        [Route("phase/generate-phase.html")]
        public async Task<IActionResult> GeneratePhaseView(int phaseId, int projectId)
        {
            PhaseViewModel viewModel = new PhaseViewModel();
            PhaseModel phase = await EzTask.Phase.GetPhaseById(phaseId);
            if (phase == null)
            {
                phase = new PhaseModel();
            }

            viewModel.Status = phase.Status.ToInt16<PhaseStatus>();
            viewModel.PhaseId = phaseId;
            viewModel.ProjectId = projectId;
            viewModel.IsDefault = phase.IsDefault;
            viewModel.PhaseName = phase.PhaseName;
            if (!viewModel.IsDefault)
            {
                viewModel.StartDate = phase.StartDate.Value.ToString("dd/MM/yyyy");
                viewModel.EndDate = phase.EndDate.Value.ToString("dd/MM/yyyy");
            }                
            
            viewModel.StatusList = StaticResources.BuildPhaseStatusSelectList(viewModel.Status);

            return PartialView("_CreateOrUpdatePhase", viewModel);
        }

        [HttpPost]
        [Route("phase/phase-modal-action.html")]
        public async Task<IActionResult> CreateOrUpdatePhase(PhaseViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            PhaseModel model = new PhaseModel()
            {
                StartDate = DateTimeUtilities.ParseFromString(viewmodel.StartDate),
                EndDate = DateTimeUtilities.ParseFromString(viewmodel.EndDate),
                Id = viewmodel.PhaseId,
                PhaseName = viewmodel.PhaseName,
                ProjectId = viewmodel.ProjectId,
                IsDefault = viewmodel.IsDefault,
                Status = viewmodel.Status.ToEnum<PhaseStatus>()
            };

            if (model.EndDate < model.StartDate)
            {
                return BadRequest("End Date must be larger than Start Date");
            }

            ResultModel<PhaseModel> iResult = await EzTask.Phase.Save(model);
            if (iResult.Status == ActionStatus.Ok)
            {
                return await LoadPhaseListAsync(model.ProjectId);
            }

            return BadRequest("Cannot create phase, please try again!");
        }

        [HttpGet]
        [Route("phase/phase-list.html")]
        public async Task<IActionResult> LoadPhaseListAsync(int projectId)
        {       
            var project = await EzTask.Project.GetProject(projectId);

            if (project!=null 
                && project.Owner.AccountId == Context.CurrentAccount.AccountId)
            {              
                Context.AddResponseHeader("authorized-add-phase", "authorized");
            }

            return ViewComponent("PhaseList", projectId);
        }

        [HttpPost]
        [Route("phase/delete-phase.html")]
        public async Task<IActionResult> RemovePhase(int phaseId)
        {
            PhaseModel phase = await EzTask.Phase.GetPhaseById(phaseId);

            if (phase != null)
            {
                if (phase.IsDefault)
                {
                    return BadRequest("You cannot delete \"Open Features\"!.");
                }
                ResultModel<bool> deleteTaskResult = await EzTask.Task.DeleteTask(phase.ProjectId, phaseId);

                if (deleteTaskResult.Data)
                {
                    ResultModel<PhaseModel> iResult = await EzTask.Phase.Delele(phase);

                    if (iResult.Status == ActionStatus.Ok)
                    {
                        return Ok();
                    }
                }
            }
            else
            {
                return BadRequest("Phase doesn't exist!.");
            }

            return BadRequest("Error, cannot process your request!.");
        }

        [HttpPost]
        [Route("phase/generate-addbutton.html")]
        public async Task<IActionResult> RenderAddNewPhaseButton()
        {
            var template = await Context.RenderViewToStringAsync("_ButtonAddNewPhase", ControllerContext);
            return Content(template);
        }
    }
}
