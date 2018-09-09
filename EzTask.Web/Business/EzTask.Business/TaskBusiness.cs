using EzTask.Entity.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class TaskBusiness : BusinessCore
    {

        public TaskBusiness(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region Task

        public async Task<ResultModel<TaskItemModel>> GetTask(int id)
        {
            ResultModel<TaskItemModel> result = new ResultModel<TaskItemModel>();
            TaskItem data = await UnitOfWork.TaskRepository.Entity
                         .Include(c => c.Project)
                         .Include(c => c.Member)
                         .Include(c => c.Assignee)
                         .Include(c => c.Phrase)
                         .AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            result.Data = data.ToModel();
            return result;
        }

        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResultModel<TaskItemModel>> SaveTask(TaskItemModel model)
        {
            ResultModel<TaskItemModel> result = new ResultModel<TaskItemModel>();

            TaskItem task = model.ToEntity();

            //reset navigate object
            task.Phrase = null;
            task.Project = null;
            task.Assignee = null;
            task.Member = null;
            task.UpdatedDate = DateTime.Now;
            if (task.AssigneeId == 0)
            {
                task.AssigneeId = null;
            }

            if (task.Id < 1)
            {
                task.TaskCode = string.Empty;
                task.CreatedDate = DateTime.Now;
                UnitOfWork.TaskRepository.Add(task);
            }
            else
            {
                UnitOfWork.TaskRepository.Update(task);
            }


            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                if (string.IsNullOrEmpty(task.TaskCode))
                {
                    task.TaskCode = CreateCode("T", task.Id);
                    int updatedRecord = await UnitOfWork.CommitAsync();
                }

                result.Status = ActionStatus.Ok;
                result.Data = task.ToModel();
            }

            return result;
        }

        /// <summary>
        /// Delete tasks
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> DeleteTask(int[] ids)
        {
            ResultModel<bool> result = new ResultModel<bool>
            {
                Data = true
            };

            IEnumerable<TaskItem> data = await UnitOfWork.TaskRepository.GetManyAsync(c => ids.Contains(c.Id));
            await DeleteTasks(result, data);

            return result;
        }

        /// <summary>
        /// Delete tasks by projectid
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> DeleteTask(int projectId)
        {
            ResultModel<bool> result = new ResultModel<bool>
            {
                Data = true
            };

            IEnumerable<TaskItem> data = await UnitOfWork.TaskRepository.GetManyAsync(c => c.ProjectId == projectId);
            await DeleteTasks(result, data);
            return result;
        }

        /// <summary>
        /// Get task list
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskItemModel>> GetTasks(int projectId,
            int phraseId,
            int page = 1,
            int pageSize = 9999)
        {

            List<TaskItem> data = await UnitOfWork.TaskRepository.Entity
                        .Include(c => c.Project)
                        .Include(c => c.Member)
                        .Include(c => c.Assignee)
                        .Include(c => c.Phrase)
                        .AsNoTracking()
                        .Where(c => c.ProjectId == projectId && c.PhraseId == phraseId)
                        .Skip(pageSize * page - pageSize).Take(pageSize)
                        .Select(x => new TaskItem
                        {
                            Phrase = new Phrase { PhraseName = x.Phrase.PhraseName, Id = x.Phrase.Id },
                            Attachments = x.Attachments.Any() ?
                                new List<Attachment>
                                {
                                    new Attachment
                                    {
                                        Id =x.Attachments.FirstOrDefault().Id
                                    }
                                } : new List<Attachment>(),
                            AssigneeId = x.AssigneeId,
                            CreatedDate = x.CreatedDate,
                            UpdatedDate = x.UpdatedDate,
                            MemberId = x.MemberId,
                            PhraseId = x.PhraseId,
                            Priority = x.Priority,
                            ProjectId = x.ProjectId,
                            Status = x.Status,
                            TaskCode = x.TaskCode,
                            TaskTitle = x.TaskTitle,
                            Id = x.Id,

                            Member = new Account
                            {
                                AccountInfo = new AccountInfo
                                {
                                    DisplayName = x.Member.AccountInfo.DisplayName
                                }
                            },
                            Assignee = new Account
                            {
                                AccountInfo = new AccountInfo
                                {
                                    DisplayName = x.Assignee != null ?
                                        x.Assignee.AccountInfo.DisplayName : "Non Assigned"
                                }
                            }
                        }).ToListAsync();

            IEnumerable<TaskItemModel> model = data.ToModels();

            return model;
        }

        /// <summary>
        /// Assign task to user
        /// </summary>
        /// <param name="taskids"></param>
        /// <param name="accountId"></param>
        public async Task AssignTask(int[] taskids, int accountId)
        {
            var tasks = await UnitOfWork.TaskRepository.GetManyAsync(c => taskids.Contains(c.Id), false);
            foreach(var task in tasks)
            {
                task.AssigneeId = accountId == 0 ? null : (int?)accountId;

                UnitOfWork.TaskRepository.Update(task);
            }
            var iResult = await UnitOfWork.CommitAsync();            
        }

        #endregion Task


        #region Attachment

        /// <summary>
        /// Get attachment list
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AttachmentModel>> GetAttachments(int taskId)
        {
            List<Attachment> iResult = await UnitOfWork.AttachRepository.Entity.Include(c => c.Task).
                Include(c => c.User).ThenInclude(c => c.AccountInfo).AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.AddedDate).ToListAsync();

            return iResult.ToModels();
        }

        /// <summary>
        /// Save an attachment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<AttachmentModel>> SaveAttachment(AttachmentModel model)
        {
            ResultModel<AttachmentModel> result = new ResultModel<AttachmentModel>();

            Attachment entity = model.ToEntity();
            byte[] file = model.FileData;

            if (entity.Id <= 0)
            {
                entity.AddedDate = DateTime.Now;
            }
            entity.Task = null;
            entity.User = null;

            UnitOfWork.AttachRepository.Add(entity);
            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = entity.ToModel();
            }

            return result;
        }

        #endregion

        #region History

        /// <summary>
        /// Load history detail
        /// </summary>
        /// <param name="historyId"></param>
        /// <returns></returns>
        public async Task<ResultModel<TaskHistoryModel>> LoadHistoryDetail(int historyId)
        {
            ResultModel<TaskHistoryModel> result = new ResultModel<TaskHistoryModel>();
            TaskHistory data = await UnitOfWork.TaskHistoryRepository.Entity.AsNoTracking()
                                .Include(c => c.Task).FirstOrDefaultAsync(c => c.Id == historyId);
            result.Data = data.ToModel();
            return result;
        }

        /// <summary>
        /// Save history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<TaskHistoryModel>> SaveHistory(TaskHistoryModel model)
        {
            ResultModel<TaskHistoryModel> result = new ResultModel<TaskHistoryModel>();

            TaskHistory entity = model.ToEntity();

            if (entity.Id <= 0)
            {
                entity.UpdatedDate = DateTime.Now;
            }
            entity.Task = null;
            entity.User = null;

            UnitOfWork.TaskHistoryRepository.Add(entity);
            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = entity.ToModel();
            }

            return result;
        }

        /// <summary>
        /// Get task history
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskHistoryModel>> GetHistoryList(int taskId, int accountId)
        {
            List<TaskHistory> iResult = await UnitOfWork.TaskHistoryRepository.Entity.Include(c => c.Task).
                Include(c => c.User).ThenInclude(c => c.AccountInfo).AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.UpdatedDate).ToListAsync();

            return iResult.ToModels();
        }

        public string CompareChanges(TaskItemModel newData, TaskItemModel oldData)
        {
           
            string content = string.Empty;
            if (newData.TaskTitle != oldData.TaskTitle)
            {
                content += $"<p><b>Title</b>:<br/><small>{oldData.TaskTitle} => {newData.TaskTitle}</small> </p>";
            }
            if (newData.TaskDetail != oldData.TaskDetail)
            {
                content += $"<p><b>Detail</b>:<br/><small>{oldData.TaskDetail} => {newData.TaskDetail}</small> </p>";
            }
            if (newData.Phrase.Id != oldData.Phrase.Id)
            {
                string oldItem = oldData.Phrase.PhraseName;
                string newItem = newData.Phrase.PhraseName;
                content += $"<p><b>Phrase</b>:<br/><small>{oldItem} => {newItem}</small> </p>";
            }
            if (newData.Assignee.AccountId != oldData.Assignee.AccountId)
            {
                string oldItem = oldData.Assignee.DisplayName;
                string newItem = newData.Assignee.DisplayName;
                content += $"<p><b>Assignee</b>:<br/><small>{oldItem} => {oldItem}</small> </p>";
            }
            if (newData.Priority != oldData.Priority)
            {
                string oldItem = oldData.Priority.ToString();
                string newItem = newData.Priority.ToString();
                content += $"<p><b>Priority</b>:<br/><small>{oldItem} => {oldItem}</small> </p>";
            }
            if (newData.Status != oldData.Status)
            {
                string oldItem = oldData.Status.ToString();
                string newItem = newData.Status.ToString();
                content += $"<p><b>Status</b>:<br/><small>{oldItem} => {oldItem}</small> </p>";
            }

            return content;
        }
        #endregion

        #region Private

        private async Task DeleteTasks(ResultModel<bool> result, IEnumerable<TaskItem> data)
        {
            if (!data.Any())
            {
                result.Status = ActionStatus.NotFound;
            }
            else
            {
                foreach (TaskItem task in data)
                {
                    IEnumerable<TaskHistory> history = UnitOfWork.TaskHistoryRepository.GetMany(c => c.TaskId == task.Id);
                    if (history.Any())
                    {
                        UnitOfWork.TaskHistoryRepository.DeleteRange(history);
                    }

                    IEnumerable<Attachment> attachment = UnitOfWork.AttachRepository.GetMany(c => c.TaskId == task.Id);
                    if (attachment.Any())
                    {
                        UnitOfWork.AttachRepository.DeleteRange(attachment);
                    }
                }

                UnitOfWork.TaskRepository.DeleteRange(data);
                int iResult = await UnitOfWork.CommitAsync();
                if (iResult > 0)
                {
                    result.Status = ActionStatus.Ok;
                }
            }
        }
        #endregion
    }
}