using System;
using System.Threading.Tasks;
using EzTask.Framework.Web.Filters;
using EzTask.Management.Models.Project;
using Microsoft.AspNetCore.Mvc;
using EzTask.Management.Infrastructures;
using EzTask.Framework.Message;
using EzTask.Entity.Framework;
using EzTask.Management.Models.Account;
using AutoMapper;
using System.Collections.Generic;
using EzTask.Framework.Common;
using System.Linq;

namespace EzTask.Management.Controllers
{
    [TypeFilter(typeof(AuthorizeFilter))]
    public class ProjectController : EzTaskController
    {
        public ProjectController(IServiceProvider serviceProvider, IMapper mapper) :
            base(serviceProvider, mapper)
        {
        }

        [Route("project.html")]
        public async Task<IActionResult> Index()
        {
            PageTitle = "Projects";
            var models = await GetProjectList();
            return View(models);
        }

        #region Create project 

        /// <summary>
        /// Create project view
        /// </summary>
        /// <returns></returns>
        [Route("project/create-project.html")]
        public IActionResult CreateNew()
        {
            PageTitle = "Create new project";
            return View(new ProjectModel
            {
                ActionType = ActionType.CreateNew,
                Owner = new AccountModel()
            });
        }

        /// <summary>
        /// Create success project view
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        [Route("project/create-success.html")]
        public async Task<IActionResult> CreateSuccess(string code)
        {
            PageTitle = "Creating project is successful";
            var project = await EzTask.Project.GetProjectDetail(code);

            return View(project.MapToModel());
        }

        /// <summary>
        /// Create project action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("project/create-project.html")]
        public async Task<IActionResult> CreateNew(ProjectModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isDupplicated = await EzTask.Project.IsDupplicated(model.ProjectName, model.ProjectId);

                    if (isDupplicated)
                    {
                        ErrorMessage = ProjectMessage.ProjectIsDupplicated;
                    }
                    else
                    {
                        model.ProjectCode = string.Empty;
                        model.Status = ProjectStatus.Pending;
                        model.Owner = new AccountModel
                        {
                            AccountId = AccountId
                        };

                        var project = await EzTask.Project.Save(model.MapToEntity());
                        if (project == null)
                        {
                            ErrorMessage = ProjectMessage.ErrorCreateProject;
                            model.HasError = true;
                        }
                        else
                        {
                            SuccessMessage = ProjectMessage.CreateProjectSuccess;
                            return RedirectToAction("CreateSuccess",
                                new { code = project.ProjectCode });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                ErrorMessage = ProjectMessage.ErrorCreateProject;
            }
            return View(model);

        }
        #endregion

        #region Edit project

        /// <summary>
        /// Update project view
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        [Route("project/update-project.html")]
        public async Task<IActionResult> Update(string code)
        {
            PageTitle = $"Update project: {code}";

            var project = await EzTask.Project.GetProject(code);
            if (project == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            var model = project.MapToModel();
            model.ActionType = ActionType.Update;
            return View(model);
        }

        /// <summary>
        /// update success project view
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("project/update-success.html")]
        public async Task<IActionResult> UpdateSuccess(string code)
        {
            PageTitle = "Updating project is successful";
            var project = await EzTask.Project.GetProjectDetail(code);

            return View(project.MapToModel());
        }

        /// <summary>
        /// Update project action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("project/update-project.html")]
        public async Task<IActionResult> Update(ProjectModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await EzTask.Project.GetProject(model.ProjectCode);
                    if (data == null)
                    {
                        ErrorMessage = ProjectMessage.ErrorUpdateProject;
                        model.HasError = true;
                    }
                    else
                    {
                        var isDupplicated = await EzTask.Project.IsDupplicated(model.ProjectName, model.ProjectId);

                        if (isDupplicated)
                        {
                            ErrorMessage = ProjectMessage.ProjectIsDupplicated;
                        }
                        else
                        {
                            model.Owner = new AccountModel { AccountId = data.Owner };
   
                            var project = await EzTask.Project.Save(model.MapToEntity());
                            if (project == null)
                            {
                                ErrorMessage = ProjectMessage.ErrorUpdateProject;
                                model.HasError = true;
                            }
                            else
                            {
                                SuccessMessage = ProjectMessage.UpdateProjectSuccess;
                                return RedirectToAction("UpdateSuccess",
                                    new { code = project.ProjectCode });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                ErrorMessage = ProjectMessage.ErrorUpdateProject;
            }
            return View(model);

        }

        #endregion

        #region Other Action

        /// <summary>
        /// Remove project
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("project/remove.html")]
        [TokenFilter]
        public async Task<IActionResult> RemoveProject(string code)
        {
            var removeAction = await EzTask.Project.Delete(code);
            if (removeAction == ActionStatus.Ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region Non action
        /// <summary>
        /// Get project list
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private async Task<IEnumerable<List<ProjectModel>>> GetProjectList()
        {
            IEnumerable<List<ProjectModel>> models = new List<List<ProjectModel>>();
            var data = await EzTask.Project.GetProjects(AccountId);
            if (data == null)
            {
                return models;
            }
            models = data.MapToModels().ToList().SplitList(3);
            return models;
        }
        #endregion
    }
}