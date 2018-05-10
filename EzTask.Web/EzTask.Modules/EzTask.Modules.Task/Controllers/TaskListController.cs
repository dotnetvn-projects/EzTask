﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Framework.Web.Attributes;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EzTask.Modules.Core.Infrastructures;
using EzTask.Modules.Tasks.Models;

namespace EzTask.Modules.Tasks.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class TaskListController : EzTaskController
    {
        public TaskListController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("task.html")]
        public async Task<IActionResult> Index()
        {
            TaskListModel model = new TaskListModel();
            model.ProjectItems = await CreateProjectSelectList();

            return View(model);
        }

        
        #region Non Action

        private async Task<List<SelectListItem>> CreateProjectSelectList()
        {
            var data = await EzTask.Project.GetProjects(AccountId);
            var projectModels = data.MapToModels();

            List<SelectListItem> selectLists = new List<SelectListItem>();

            foreach (var pro in projectModels)
            {
                selectLists.Add(new SelectListItem
                {
                    Text = pro.ProjectName + " (" + pro.Status + ")",
                    Value = pro.ProjectId.ToString()
                });
            }
            selectLists[0].Selected = true;
            return selectLists;
        }

        #endregion
    }
}