using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Framework.Common;
using EzTask.Framework.Message;
using EzTask.Modules.Core.Controllers;
using EzTask.Models;
using Microsoft.AspNetCore.Mvc;
using EzTask.Web.Framework.Attributes;
using EzTask.Models.Enum;

namespace EzTask.Modules.Project.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class ProjectController : BaseController
    {
        public ProjectController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("project.html")]
        [PageTitle("Projects")]
        public async Task<IActionResult> Index()
        {
            var models = await GetProjectList();
            return View(models);
        }

        #region Create project 

        /// <summary>
        /// Create project view
        /// </summary>
        /// <returns></returns>
        [Route("project/create-project.html")]
        [PageTitle("Create new project")]
        public IActionResult CreateNew()
        {
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
        [PageTitle("Creating project is successful")]
        public async Task<IActionResult> CreateSuccess(string code)
        {
            var project = await EzTask.Project.GetProjectDetail(code);

            return View(project);
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

                        var project = await EzTask.Project.Save(model);
                        if (project == null)
                        {
                            ErrorMessage = ProjectMessage.ErrorCreateProject;
                            model.HasError = true;
                        }
                        else
                        {
                            SuccessMessage = ProjectMessage.CreateProjectSuccess;
                            return base.RedirectToAction("CreateSuccess",
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
        [PageTitle("Update project: ")]
        public async Task<IActionResult> Update(string code)
        {
            PageTitleAttribute.CombineWith(this, code);

            var project = await EzTask.Project.GetProject(code);
            if (project == null)
            {
                return RedirectToAction("PageNotFound", "Common");
            }
            project.ActionType = ActionType.Update;
            return View(project);
        }

        /// <summary>
        /// update success project view
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("project/update-success.html")]
        [PageTitle("Updating project is successful")]
        public async Task<IActionResult> UpdateSuccess(string code)
        {
            var project = await EzTask.Project.GetProjectDetail(code);   
            return View(project);
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
                            model.Owner = new AccountModel { AccountId = data.Owner.AccountId };

                            var project = await EzTask.Project.Save(model);
                            if (project == null)
                            {
                                ErrorMessage = ProjectMessage.ErrorUpdateProject;
                                model.HasError = true;
                            }
                            else
                            {
                                SuccessMessage = ProjectMessage.UpdateProjectSuccess;
                                return base.RedirectToAction("UpdateSuccess",
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
        [TokenAttribute]
        public async Task<IActionResult> RemoveProject(string code)
        {
            var iResult = await EzTask.Project.Delete(code);
            if (iResult.Status == ActionStatus.Ok)
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
            models = data.ToList().SplitList(3);
            return models;
        }
        #endregion
    }
}
