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
using EzTask.Framework.IO;
using System.IO;
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
        public async Task<IActionResult> GenerateTaskView(TaskFormDataModel model)
        {
            var task = new TaskItemViewModel();
            if(model.TaskId == 0)
            {
                task.ProjectId = model.ProjectId;
                task.AccountId = AccountId;
                task.PhraseId = model.PhraseId;      
                task.ActionType = ActionType.CreateNew;
            }
            else
            {
                task.ActionType = ActionType.Update;
                var iResult = await EzTask.Task.GetTask(task.TaskId);
                UpdateTaskFromExist(task, iResult);
            }

            var phrases = await EzTask.Phrase.GetPhrase(task.ProjectId);
            task.PhraseList = BuildPhraseSelectList(phrases, task.PhraseId);

            var assignees = await EzTask.Project.GetAccountList(task.ProjectId);
            task.AssigneeList = BuildAssigneeSelectList(assignees);

            task.StatusList = BuildStatusSelectList();
            task.PriorityList = BuildPrioritySelectList();           

            return PartialView("_CreateOrUpdateTask", task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNew(TaskItemViewModel viewModel)
        {
            var model = CreateTaskItemModel(viewModel);
            var iResult = await EzTask.Task.CreateTask(model);
            if(iResult.Status == ActionStatus.Ok)
            {
                string title = DisplayName + " created task \"" + iResult.Data.TaskTitle +"\"";
                await SaveTaskHistory(iResult.Data.TaskId, title, string.Empty);
            }
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
        public async Task<IActionResult> UploadAttachFile(IFormFile file, int taskId)
        {
            if (file.Length > 0)
            {
                var stream = file.OpenReadStream();

                AttachmentModel model = new AttachmentModel
                {
                    FileName = file.FileName,
                    FileData = await stream.ConvertStreamToBytes(),
                    FileType = file.ContentType,
                    Task = new TaskItemModel {TaskId = taskId },
                    User = new AccountModel { AccountId = AccountId }
                };
                var iResult = await EzTask.Task.SaveAttachment(model);

                if(iResult.Status == ActionStatus.Ok)
                {
                    string title = DisplayName + " uploaded file \"" + iResult.Data.FileName+"\"";
                    await SaveTaskHistory(taskId, title, string.Empty);
                }
                return Json(model);
            }
            return BadRequest();
        }

        /// <summary>
        /// Get attachments list return vew component
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("taskitem/attachment-list.html")]
        public IActionResult GetTaskAttachmentList(int taskId)
        {
            return ViewComponent("Attachments", new { taskId });
        }

        /// <summary>
        /// Get attachments list return vew component
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("taskitem/history-list.html")]
        public IActionResult GetTaskHistoryList(int taskId)
        {
            return ViewComponent("HistoryList", new { taskId, accountId = AccountId });
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
        /// Update from exist task model
        /// </summary>
        /// <param name="task"></param>
        /// <param name="iResult"></param>
        private static void UpdateTaskFromExist(TaskItemViewModel task, ResultModel<TaskItemModel> iResult)
        {
            task.TaskId = iResult.Data.TaskId;
            task.ProjectId = iResult.Data.Project.ProjectId;
            task.PhraseId = iResult.Data.Phrase.Id;
            task.Assignee = iResult.Data.Assignee != null ? iResult.Data.Assignee.AccountId : 0;
            task.TaskTitle = iResult.Data.TaskTitle;
            task.TaskCode = iResult.Data.TaskCode;
            task.TaskDetail = iResult.Data.TaskDetail;
            task.Status = iResult.Data.Status.ToInt16<TaskStatus>();
            task.Priority = iResult.Status.ToInt16<TaskPriority>();
            task.AccountId = iResult.Data.Member.AccountId;
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
            List<SelectListItem> assigneeItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "0",
                    Text = "Non-Assigned"
                }
            };

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