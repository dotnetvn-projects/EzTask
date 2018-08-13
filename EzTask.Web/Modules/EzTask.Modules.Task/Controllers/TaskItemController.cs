using System;
using System.Collections.Generic;
using System.Linq;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Task.Models;
using EzTask.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EzTask.Framework.Common;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace EzTask.Modules.Task.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class TaskItemController : CoreController
    {
        public TaskItemController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("taskitem/generate-view.html")]
        [HttpPost]
        public async Task<IActionResult> PrepairTaskViewItem(TaskFormDataModel model)
        {
            var newTask = new TaskItemViewModel();
            if(model.TaskId == 0)
            {
                model.ActionType = ActionType.CreateNew;
            }
            newTask.ProjectId = model.ProjectId;

            var phrases = await EzTask.Phrase.GetPhrase(model.ProjectId);
            newTask.PhraseList = BuildPhraseSelectList(phrases, model.PhraseId);

            var assignees = await EzTask.Project.GetAccountList(model.ProjectId);
            newTask.AssigneeList = BuildAssigneeSelectList(assignees);

            newTask.StatusList = BuildStatusSelectList();
            newTask.PriorityList = BuildPrioritySelectList();

            return PartialView("_CreateOrUpdateTask", newTask);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNew(TaskItemViewModel viewModel)
        {
            var model = CreateTaskItemModel(viewModel);
            var iResult = await EzTask.Task.CreateTask(model);
            return Json(iResult);
        }

        [HttpPost]
        public IActionResult Update(TaskItemViewModel model)
        {
            return Json("OK");
        }

        /// <summary>
        /// Update file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("taskitem/upload-attach-file.html")]
        public IActionResult UpdateAttachFile(IFormFile file, int taskId)
        {
            if (file.Length > 0)
            {
                var stream = file.OpenReadStream();

              

                AttachmentModel model = new AttachmentModel
                {
                    FileName = file.FileName,
                    
                };

                return Json(model);
            }
            return BadRequest();
        }

        #region Non-Action

        /// <summary>
        /// Create TaskItemModel from ViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private TaskItemModel CreateTaskItemModel(TaskItemViewModel viewModel)
        {
            var data = new TaskItemModel();
            data.Assignee.AccountId = viewModel.Assignee;
            data.Member.AccountId = AccountId;
            data.Phrase.Id = viewModel.PhraseId;
            data.Project.ProjectId = viewModel.ProjectId;
            data.Priority = viewModel.Priority.ToEnum<TaskPriority>();
            data.Status = viewModel.Status.ToEnum<TaskItemStatus>();
            data.TaskDetail = viewModel.TaskDetail;
            data.TaskCode = viewModel.TaskCode;
            data.TaskId = viewModel.TaskId;
            data.TaskTitle = viewModel.TaskTitle;

            return data;
        }

        /// <summary>
        /// Build Priority SelectList items
        /// </summary>
        /// <returns></returns>
        private static List<SelectListItem> BuildPrioritySelectList(Int16 selectedId = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var dataItems = EnumUtilities.ToList<TaskPriority>();
            if (dataItems.Any())
            {
                foreach (var data in dataItems)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = data,
                        Value = ((Int16)data.ToEnum<TaskPriority>()).ToString()
                    };

                    if (selectedId != 0)
                    {
                        var enumData = selectedId.ToEnum<TaskPriority>();
                        if (data == enumData.ToString())
                        {
                            selectItem.Selected = true;
                        }
                    }
                    list.Add(selectItem);
                }
            }
            if (selectedId == 0)
            {
                list[0].Selected = true;
            }
            return list;
        }

        /// <summary>
        /// Build Phrase SelectList items
        /// </summary>
        /// <param name="phrases"></param>
        /// <returns></returns>
        private static List<SelectListItem> BuildPhraseSelectList(IEnumerable<PhraseModel> phrases,
            int selectedId = 0)
        {
            List<SelectListItem> phraseItems = new List<SelectListItem>();

            if (phrases.Any())
            {
                foreach (var phr in phrases)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = phr.PhraseName,
                        Value = phr.Id.ToString()
                    };
                    if (phr.Id == selectedId)
                    {
                        selectItem.Selected = true;
                    }
                    phraseItems.Add(selectItem);
                }
            }
            if (selectedId == 0)
            {
                phraseItems[0].Selected = true;
            }
            return phraseItems;
        }

        /// <summary>
        /// Build Assignees SelectList items
        /// </summary>
        /// <param name="assignees"></param>
        /// <returns></returns>
        private static List<SelectListItem> BuildAssigneeSelectList(IEnumerable<AccountModel> assignees,
            int selectedId = 0)
        {
            List<SelectListItem> assigneeItems = new List<SelectListItem>();
            assigneeItems.Add(new SelectListItem
            {
                Value = "0",
                Text = "Non-Assigned"
            });

            if (assignees.Any())
            {
                foreach (var assign in assignees)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = assign.DisplayName,
                        Value = assign.AccountId.ToString()
                    };
                    if (assign.AccountId == selectedId)
                    {
                        selectItem.Selected = true;
                    }
                    assigneeItems.Add(selectItem);
                }
            }
            if (selectedId == 0)
            {
                assigneeItems[0].Selected = true;
            }
            return assigneeItems;
        }

        /// <summary>
        /// Build Status SelectList items
        /// </summary>
        /// <returns></returns>
        private static List<SelectListItem> BuildStatusSelectList(Int16 selectedId = 0)
        {
            List<SelectListItem> statusItems = new List<SelectListItem>();
            var statuses = EnumUtilities.ToList<TaskItemStatus>();
            if (statuses.Any())
            {
                foreach (var status in statuses)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = status,
                        Value = ((Int16)status.ToEnum<TaskItemStatus>()).ToString()
                    };

                    if (selectedId != 0)
                    {
                        var statusEnum = selectedId.ToEnum<TaskItemStatus>();
                        if(status == statusEnum.ToString())
                        {
                            selectItem.Selected = true;
                        }
                    } 
                    statusItems.Add(selectItem);
                }
            }
            if(selectedId == 0)
            {
                statusItems[0].Selected = true;
            }
            return statusItems;
        }
        #endregion
    }
}