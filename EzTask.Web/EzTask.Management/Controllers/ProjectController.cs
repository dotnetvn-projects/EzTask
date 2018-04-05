using System;
using System.Threading.Tasks;
using EzTask.Framework.Web.AuthorizeFilter;
using EzTask.Management.Models.Project;
using Microsoft.AspNetCore.Mvc;
using EzTask.Management.Infrastructures;
using EzTask.Framework.Message;
using EzTask.Entity.Framework;
using EzTask.Management.Models.Account;

namespace EzTask.Management.Controllers
{
    [TypeFilter(typeof(EzTaskAuthorizeFilter))]
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

        #region Create 

        [Route("project/create-project.html")]
        public IActionResult CreateNew()
        {
            PageTitle = "Create new task";
            return View(new ProjectModel());
        }

        [HttpPost]
        [Route("project/create-project.html")]
        public async Task<IActionResult> CreateNew(ProjectModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Status = ProjectStatus.Pending;
                    model.Owner = new AccountModel
                    {
                        AccountId = AccountId
                    };
                    var project = await EzTask.Project.Save(model.MapToEntity());
                }
            }
            catch
            {
                ErrorMessage = ProjectMessage.ErrorCreateProject;
            }
            return View();
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