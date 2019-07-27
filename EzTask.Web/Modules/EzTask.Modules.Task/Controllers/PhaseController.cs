using EzTask.Framework.Common;
using EzTask.Framework.Data;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Task.ViewModels;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.Data;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.Controllers
{
    [TypeFilter(typeof(ApplyLanguageAttribute))]
    [TypeFilter(typeof(AuthenticationAttribute))]
    public class PhaseController : BaseController
    {
        public PhaseController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        /// <summary>
        /// Create phase view
        /// </summary>
        /// <param name="phaseId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Phase action
        /// </summary>
        /// <param name="viewmodel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("phase/phase-modal-action.html")]
        public async Task<IActionResult> CreateOrUpdatePhase(PhaseViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Context.GetStringResource("CreatePhaseError", StringResourceType.TaskPage));
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
                return BadRequest(Context.GetStringResource("DateRange", StringResourceType.Error));
            }

            ResultModel<PhaseModel> iResult = await EzTask.Phase.Save(model);
            if (iResult.Status == ActionStatus.Ok)
            {
                return await LoadPhaseListAsync(model.ProjectId);
            }

            if(model.Id > 0)
            {
                return BadRequest(Context.GetStringResource("UpdatePhaseError", StringResourceType.TaskPage));
            }
            return BadRequest(Context.GetStringResource("CreatePhaseError", StringResourceType.TaskPage));
        }

        /// <summary>
        /// Load phase list
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete phase
        /// </summary>
        /// <param name="phaseId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("phase/delete-phase.html")]
        public async Task<IActionResult> RemovePhase(int phaseId)
        {
            PhaseModel phase = await EzTask.Phase.GetPhaseById(phaseId);

            if (phase != null)
            {
                if (phase.IsDefault)
                {
                    return BadRequest(Context.GetStringResource("DeleteOpenFeatureWarning", StringResourceType.TaskPage));
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
                return BadRequest(Context.GetStringResource("PhaseNotExist",  StringResourceType.TaskPage));
            }

            return BadRequest(Context.GetStringResource("RequestFailed", StringResourceType.Error));
        }

        /// <summary>
        /// Render add phase button depends on context
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("phase/generate-addbutton.html")]
        public async Task<IActionResult> RenderAddNewPhaseButton()
        {
            var template = await Context.RenderViewToStringAsync("_ButtonAddNewPhase", ControllerContext);
            return Content(template);
        }
    }
}
