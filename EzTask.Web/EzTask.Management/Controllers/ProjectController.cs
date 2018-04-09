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

namespace EzTask.Management.Controllers
{  
    public class ProjectController : EzTaskController
    {
        public ProjectController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("project.html")]
        public IActionResult Index()
        {
            PageTitle = "Projects";
            return View();
        }

        #region Create project 

        [Route("project/create-project.html")]
        public IActionResult CreateNew()
        {
            PageTitle = "Create new project";
            return View(new ProjectModel());
        }

        [Route("project/create-success.html")]
        public async Task<IActionResult> CreateSuccess(string projectCode)
        {
            PageTitle = "Creating project is successful";
            var project = await EzTask.Project.GetProjectDetail(projectCode);
            
            return View(project.MapToModel());
        }

        [HttpPost]
        [Route("project/create-project.html")]
        public async Task<IActionResult> CreateNew(ProjectModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.ProjectCode = string.Empty;
                    model.Status = ProjectStatus.Pending;
                    model.Owner = new AccountModel
                    {
                        AccountId = AccountId
                    };

                    var project = await EzTask.Project.Save(model.MapToEntity());
                    if(project == null)
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
            catch
            {
                model.HasError = true;
                ErrorMessage = ProjectMessage.ErrorCreateProject;              
            }
            return View(model);

        }
        #endregion

        #region Edit

        [Route("update-task.html")]
        public async Task<IActionResult> UpdateProject(string projectCode)
        {
            PageTitle = $"Update project: {projectCode}";

            var project = await EzTask.Project.GetProject(projectCode);
            if (project == null)
            {
                return NotFound();
            }

            return View(project.MapToModel());
        }

        #endregion
    }
}