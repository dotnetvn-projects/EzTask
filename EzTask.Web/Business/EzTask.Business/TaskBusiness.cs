using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Interfaces;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class TaskBusiness : BusinessCore
    {

        public TaskBusiness(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region Task

        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResultModel<TaskItemModel>> CreateTask(TaskItemModel model)
        {
            ResultModel<TaskItemModel> result = new ResultModel<TaskItemModel>();

            var task = model.ToEntity();

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
            var iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                if (string.IsNullOrEmpty(task.TaskCode))
                {
                    task.TaskCode = CreateCode("T", task.Id);
                    var updatedRecord = await UnitOfWork.CommitAsync();
                }

                result.Status = ActionStatus.Ok;
                result.Data = task.ToModel();
            }

            return result;
        }

        public async Task<ResultModel<bool>> DeleteTask(int[] ids)
        {
            ResultModel<bool> result = new ResultModel<bool>
            {
                Data = true
            };

            var data = await UnitOfWork.TaskRepository.GetManyAsync(c => ids.Contains(c.Id));
            if (!data.Any())
            {
                result.Status = ActionStatus.NotFound;
            }
            else
            {
                UnitOfWork.TaskRepository.DeleteRange(data);
                var iResult = await UnitOfWork.CommitAsync();
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
        public async Task<IEnumerable<TaskItemModel>> GetTasks(int projectId, int phraseId,
            int page = 1, int pageSize = 1000)
        {

            var data = await UnitOfWork.TaskRepository.Entity.Include(c => c.Project)
                                   .Include(c => c.Member)
                                   .Include(c => c.Assignee)
                                   .Include(c => c.Phrase)
                                   .AsNoTracking()
                                   .Where(c => c.ProjectId == projectId && c.PhraseId == phraseId)
                                   .Skip(pageSize * page - pageSize).Take(pageSize)
                                   .ToListAsync();

            var model = data.ToModels();

            foreach (var item in model)
            {
                if (item.Assignee == null ||
                    string.IsNullOrEmpty(item.Assignee.DisplayName))
                {
                    item.Assignee = new AccountModel
                    {
                        DisplayName = "Non Assigned"
                    };
                }
            }
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
            var iResult = await UnitOfWork.AttachRepository.Entity.Include(c => c.Task).
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

            var entity = model.ToEntity();
            var file = model.FileData;

            if (entity.Id <= 0)
            {
                entity.AddedDate = DateTime.Now;
            }
            entity.Task = null;
            entity.User = null;
            UnitOfWork.AttachRepository.Add(entity);
            var iResult = await UnitOfWork.CommitAsync();

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
        /// Get task history
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskHistoryModel>> GetHistoryList(int taskId, int accountId)
        {
            var iResult = await UnitOfWork.TaskHistoryRepository.Entity.Include(c => c.Task).
                Include(c => c.User).ThenInclude(c => c.AccountInfo).AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.UpdatedDate).ToListAsync();

            return iResult.ToModels();
        }
        #endregion
    }
}