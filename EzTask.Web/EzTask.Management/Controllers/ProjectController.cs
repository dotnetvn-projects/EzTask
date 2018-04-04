using System;
using System.Threading.Tasks;
using EzTask.Framework.Web.AuthorizeFilter;
using EzTask.Management.Models.Project;
using Microsoft.AspNetCore.Mvc;
using EzTask.Management.Infrastructures;

namespace EzTask.Management.Controllers
{
    [TypeFilter(typeof(EzTaskAuthorizeFilter))]
    public class ProjectController : EzTaskController
    {
        public ProjectController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("projects.html")]
        public IActionResult Index()
        {
            PageTitle = "Projects";
            return View();
        }

        #region Create 

        [Route("create-task.html")]
        public IActionResult CreateNew()
        {
            PageTitle = "Create new task";
            return View(new ProjectModel());
        }

        [HttpPost]
        [Route("create-task.html")]
        public async Task<IActionResult> CreateNew(ProjectModel model)
        {
            if (ModelState.IsValid)
            {
                var project = await EzTask.Project.Save(model.MapToEntity());
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