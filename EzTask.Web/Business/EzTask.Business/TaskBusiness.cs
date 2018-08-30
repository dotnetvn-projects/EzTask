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
            var data = await UnitOfWork.TaskRepository.GetByIdAsync(id, false);
            result.Data = data.ToModel();
            return result;
        }

        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResultModel<TaskItemModel>> CreateTask(TaskItemModel model)
        {
            ResultModel<TaskItemModel> result = new ResultModel<TaskItemModel>();

            TaskItem task = model.ToEntity();

            //reset navigate object
            task.Phrase = null;
            task.Project = null;
            task.Assignee = null;
            task.Member = null;

            if (task.AssigneeId == 0)
            {
                task.AssigneeId = null;
            }

            if (task.Id < 1)
            {
                task.TaskCode = string.Empty;
                task.CreatedDate = DateTime.Now;
            }

            task.UpdatedDate = DateTime.Now;

            UnitOfWork.TaskRepository.Add(task);
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
            int pageSize = 50)
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
                            Phrase = new Phrase { PhraseName = x.Phrase.PhraseName},
                            Attachments = x.Attachments.Any()? 
                                new List<Attachment>
                                {
                                    new Attachment
                                    {
                                        Id =x.Attachments.FirstOrDefault().Id
                                    }
                                } : new List<Attachment> (),
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
        #endregion
    }
}