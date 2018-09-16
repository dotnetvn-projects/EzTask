﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EzTask.Modules.Task.Models;
using EzTask.Web.Framework.Attributes;
using System.Linq;
using EzTask.Models;
using EzTask.Web.Framework.Data;

namespace EzTask.Modules.Task.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class TaskController : BaseController
    {
        public TaskController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("task.html")]
        public async Task<IActionResult> Index()
        {
            TaskViewModel model = await PrepareData();        
            return View(model);
        }

        #region Api

        /// <summary>
        /// Get task list return vew component
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("task/task-list.html")]
        public IActionResult GetTaskList(int projectId, int phraseId)
        {
            return ViewComponent("TaskList", new { projectId, phraseId });
        }

        /// <summary>
        /// Delete tasks by id range
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("task/delete-task.html")]
        public async Task<IActionResult> DeleteTasks(int[] taskIds)
        {
            var iResult = await EzTask.Task.DeleteTask(taskIds);
            return Json(iResult);
        }

        /// <summary>
        /// Generate assign task view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("task/generate-assign-view.html")]
        [HttpPost]
        public async Task<IActionResult> GenerateAssignTaskView(TaskFormDataModel model)
        {
            var task = new TaskItemViewModel();

            var assignees = await EzTask.Project.GetAccountList(model.ProjectId);
            task.AssigneeList = StaticResources.BuildAssigneeSelectList(assignees);

            return PartialView("_AssignTask", task);
        }

        /// <summary>
        /// Assign tasks to user
        /// </summary>
        /// <param name="taskids"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("task/assign-task.html")]
        public async Task<IActionResult> Assign(int[] taskids, int accountId)
        {
            if (taskids != null && taskids.Length > 0)
            {
                await EzTask.Task.AssignTask(taskids, accountId);
            }

            return Ok();
        }

        #endregion

        #region Non Action

        /// <summary>
        /// Create project select list
        /// </summary>
        /// <returns></returns>
        private async Task<TaskViewModel> PrepareData()
        {
            TaskViewModel viewModel = new TaskViewModel();
            var projects = await EzTask.Project.GetProjects(AccountId);

            if (projects.Any())
            {
                viewModel.Project = projects.First();
                var features = await EzTask.Phrase.GetOpenFeaturePhrase(viewModel.Project.ProjectId);
                viewModel.Phrase = features;
            }
          
            viewModel.ProjectItems = BuildProjectSelectList(projects);

            return viewModel;
        }

        /// <summary>
        /// Build project SelectList items
        /// </summary>
        /// <param name="projects"></param>
        /// <returns></returns>
        private static List<SelectListItem> BuildProjectSelectList(
            IEnumerable<ProjectModel> projects)
        {
            List<SelectListItem> projectItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "----",
                    Value = "0",
                    Selected = true
                }
            };

            if (projects.Any())
            {
                foreach (var pro in projects)
                {
                    projectItems.Add(new SelectListItem
                    {
                        Text = pro.ProjectName + " (" + pro.Status + ")",
                        Value = pro.ProjectId.ToString()
                    });
                }
            }
            
            if (projectItems.Count > 1)
                projectItems[1].Selected = true;

            return projectItems;
        }

        #endregion
    }
}
