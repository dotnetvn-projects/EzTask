using System;
using System.Threading.Tasks;
using EzTask.Framework.Web.AuthorizeFilter;
using EzTask.Management.Models.Project;
using Microsoft.AspNetCore.Mvc;
using EzTask.Management.Infrastructures;
using EzTask.Framework.Message;
using EzTask.Entity.Framework;
using EzTask.Management.Models.Account;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using EzTask.Management.Models;
using AutoMapper;

namespace EzTask.Management.Controllers
{
    [TypeFilter(typeof(EzTaskAuthorizeFilter))]
    public class ProjectController : EzTaskController
    {
        public ProjectController(IServiceProvider serviceProvider, IMapper mapper) :
            base(serviceProvider, mapper)
        {
        }

        [Route("project.html")]
        public Task<IActionResult> Index()
        {
            PageTitle = "Projects";
            return View();
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
            return View(new ProjectModel());
        }

        /// <summary>
        /// Create success project view
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        [Route("project/create-success.html")]
        public async Task<IActionResult> CreateSuccess(string projectCode)
        {
            PageTitle = "Creating project is successful";
            var project = await EzTask.Project.GetProjectDetail(projectCode);
            
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
                    var isDupplicatedName = await EzTask.Project.GetProjectByName(model.ProjectName);

                    if (isDupplicatedName != null)
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
                                new { projectCode = project.ProjectCode });
                        }
                    }
                }
            }
            catch
            {
                model.HasError = true;
                ErrorMessage = ProjectMessage.ErrorCreateProject;              
            }
            return View(model);

        }
        #endregion

        #region Edit

        /// <summary>
        /// Update project view
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        [Route("update-project.html")]
        public async Task<IActionResult> UpdateProject(string projectCode)
        {
            PageTitle = $"Update project: {projectCode}";

            var project = await EzTask.Project.GetProject(projectCode);
            if (project == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            return View(project.MapToModel());
        }

        #endregion
    }
}