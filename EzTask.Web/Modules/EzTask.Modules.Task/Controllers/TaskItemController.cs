using System;
using System.Collections.Generic;
using System.Linq;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Task.ViewModels;
using EzTask.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EzTask.Framework.Common;
using System.Threading.Tasks;
using EzTask.Framework.IO;
using System.IO;
using Microsoft.AspNetCore.Http;
using EzTask.Framework.Data;
using System.Text;
using EzTask.Web.Framework.Data;
using EzTask.Web.Framework.WebContext;

namespace EzTask.Modules.Task.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class TaskItemController : BaseController
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
            if (model.TaskId == 0)
            {
                task.ProjectId = model.ProjectId;
                task.AccountId = Context.CurrentAccount.AccountId;
                task.PhaseId = model.PhaseId;
            }
            else
            {
                var iResult = await EzTask.Task.GetTask(model.TaskId);
                UpdateTaskFromExist(task, iResult);
            }

            var phases = await EzTask.Phase.GetPhases(task.ProjectId);
            task.PhaseList = StaticResources.BuildPhaseSelectList(phases, task.PhaseId);

            var assignees = await EzTask.Project.GetAccountList(task.ProjectId);
            task.AssigneeList = StaticResources.BuildAssigneeSelectList(assignees, task.Assignee);

            task.StatusList = StaticResources.BuildTaskStatusSelectList(task.Status);
            task.PriorityList = StaticResources.BuildPrioritySelectList(task.Priority);

            return PartialView("_CreateOrUpdateTask", task);
        }

        /// <summary>
        /// Create or Update task action
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(TaskItemViewModel viewModel)
        {
            var model = CreateTaskItemModel(viewModel);

            var iResult = await EzTask.Task.SaveTask(model);

            if (iResult.Status == ActionStatus.Ok)
            {
                model.TaskCode = iResult.Data.TaskCode;
                model.CreatedDate = iResult.Data.CreatedDate;
                model.UpdatedDate = iResult.Data.UpdatedDate;
                model.TaskId = iResult.Data.TaskId;

                string title = string.Empty;
                string diff = string.Empty;

                if (viewModel.TaskId <= 0)
                {
                    title = Context.GetStringResource("CreatedTask", StringResourceType.TaskPage) + " \"" + iResult.Data.TaskTitle + "\" <small>("+ Context.GetStringResource("Code", StringResourceType.TaskPage) + ": " + model.TaskCode + ")</small>";                   
                    SetTaskDataToSession(model);

                    //add notify
                    await EzTask.Notification.AddNewTaskNotify(Context.CurrentAccount.DisplayName,
                        Context.CurrentAccount.AccountId,
                        iResult.Data.TaskCode, viewModel.ProjectId,
                        Context.GetStringResource("AddNewTask", StringResourceType.Notification));
                }
                else
                {
                    title = Context.GetStringResource("UpdatedTask", StringResourceType.TaskPage) + " \"" + iResult.Data.TaskTitle + "\" <small>(" + Context.GetStringResource("Code", StringResourceType.TaskPage) + ": " + model.TaskCode + ")</small>";
                    var oldData = ReadTaskDataFromSession(model.TaskId);
                    var newData = model;

                    diff = await EzTask.Task.CompareChangesAsync(newData, oldData);

                    //re-assign new data to session
                    SetTaskDataToSession(newData);

                    //add notify
                    await EzTask.Notification.UpdateTaskNotify(Context.CurrentAccount.DisplayName,
                        Context.CurrentAccount.AccountId,
                        iResult.Data.TaskCode, viewModel.ProjectId,
                        Context.GetStringResource("UpdateTask", StringResourceType.Notification));
                }

                await EzTask.Task.SaveHistory(iResult.Data.TaskId, title, diff, Context.CurrentAccount.AccountId);

            }
            return Json(iResult);
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
                    Task = new TaskItemModel { TaskId = taskId },
                    User = new AccountModel { AccountId = Context.CurrentAccount.AccountId }
                };
                var iResult = await EzTask.Task.SaveAttachment(model);

                if (iResult.Status == ActionStatus.Ok)
                {
                    string title = string.Format(Context.CurrentAccount.DisplayName + " {0} \"" + iResult.Data.FileName + "\"",
                                                    Context.GetStringResource("UploadedFile", StringResourceType.TaskPage));
                    await EzTask.Task.SaveHistory(taskId, title, string.Empty, Context.CurrentAccount.AccountId);
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
            return ViewComponent("HistoryList", new { taskId, accountId = Context.CurrentAccount.AccountId });
        }

        [HttpPost]
        [Route("taskitem/history-detail.html")]
        public async Task<IActionResult> LoadHistoryDetail(int historyId)
        {
            var iResult = await EzTask.Task.LoadHistoryDetail(historyId);
            return PartialView("_HistoryDetail", iResult.Data);
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
            data.Member.AccountId = Context.CurrentAccount.AccountId;
            data.Phase.Id = viewModel.PhaseId;
            data.Project.ProjectId = viewModel.ProjectId;
            data.Priority = viewModel.Priority.ToEnum<TaskPriority>();
            data.Status = viewModel.Status.ToEnum<TaskItemStatus>();
            data.TaskDetail = viewModel.TaskDetail;
            data.TaskCode = viewModel.TaskCode;
            data.TaskId = viewModel.TaskId;
            data.TaskTitle = viewModel.TaskTitle;
            data.CreatedDate = viewModel.CreatedDate;
            data.PercentCompleted = viewModel.PercentCompleted;

            return data;
        }

        /// <summary>
        /// Update from exist task model
        /// </summary>
        /// <param name="task"></param>
        /// <param name="iResult"></param>
        private void UpdateTaskFromExist(TaskItemViewModel task, ResultModel<TaskItemModel> iResult)
        {
            task.TaskId = iResult.Data.TaskId;
            task.ProjectId = iResult.Data.Project.ProjectId;
            task.PhaseId = iResult.Data.Phase.Id;
            task.Assignee = iResult.Data.Assignee != null ? iResult.Data.Assignee.AccountId : 0;
            task.TaskTitle = iResult.Data.TaskTitle;
            task.TaskCode = iResult.Data.TaskCode;
            task.TaskDetail = iResult.Data.TaskDetail;
            task.Status = iResult.Data.Status.ToInt16<TaskStatus>();
            task.Priority = iResult.Status.ToInt16<TaskPriority>();
            task.AccountId = iResult.Data.Member.AccountId;
            task.PercentCompleted = iResult.Data.PercentCompleted;
            task.CreatedDate = iResult.Data.CreatedDate;

            if (task.TaskId > 0)
            {
                SetTaskDataToSession(CreateTaskItemModel(task));
            }
        }

        /// <summary>
        /// Get data from session to serve for comparing
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        private TaskItemModel ReadTaskDataFromSession(int taskId)
        {
            TaskItemModel result = new TaskItemModel();
            var data = SessionManager.GetObject<List<TaskItemModel>>(SessionKey.TrackTask);
            if (data != null)
            {
                var item = data.FirstOrDefault(c => c.TaskId == taskId);
                if (item != null)
                {
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Store data to session to serve for comparing
        /// </summary>
        /// <param name="taskData"></param>
        private void SetTaskDataToSession(TaskItemModel taskData)
        {
            TaskItemModel result = new TaskItemModel();
            var data = SessionManager.GetObject<List<TaskItemModel>>(SessionKey.TrackTask);
            if (data == null)
            {
                data = new List<TaskItemModel>();
            }
            else
            {
                var item = data.FirstOrDefault(c => c.TaskId == taskData.TaskId);
                if (item != null)
                {
                    data.Remove(item);
                }
            }

            data.Add(taskData);
            SessionManager.SetObject(SessionKey.TrackTask, data);
        }

        #endregion
    }
}