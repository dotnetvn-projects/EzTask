using EzTask.Framework.Infrastructures;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class NotificationBusiness : BusinessCore
    {
        private readonly ProjectBusiness _project;
        private readonly TaskBusiness _task;

        public NotificationBusiness(UnitOfWork unitOfWork,
            ProjectBusiness project, TaskBusiness task) : base(unitOfWork)
        {
            _project = project;
            _task = task;
        }

        #region Task Notification

        /// <summary>
        /// add new notification to all member of project when there is someone create new task
        /// </summary>
        /// <param name="member"></param>
        /// <param name="taskCode"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task AddNewTaskNotify(string accountName,int accountId,
            string taskCode, int projectId, string contentTemplate)
        {
            var memberIds = await _project.GetAccountIdList(projectId);
            if (memberIds.Any())
            {
                foreach(var id in memberIds)
                {
                    if (id == accountId)
                        continue;

                    await AddNotification(new NotificationModel
                    {
                        Account = new AccountModel { AccountId = id },
                        Context = NotifyContext.AddNewTask,
                        Content = $"{accountName} {contentTemplate} <b>${taskCode}</b>",
                        HasViewed = false,
                        RefData = taskCode
                    });
                }
            }         
        }

        /// <summary>
        /// add new notification to all member of project when there is someone update task
        /// </summary>
        /// <param name="member"></param>
        /// <param name="taskCode"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task UpdateTaskNotify(string accountName, int accountId,
            string taskCode, int projectId, string contentTemplate)
        {
            var memberIds = await _project.GetAccountIdList(projectId);
            if (memberIds.Any())
            {
                foreach (var id in memberIds)
                {
                    if (id == accountId)
                        continue;

                    await AddNotification(new NotificationModel
                    {
                        Account = new AccountModel { AccountId = id },
                        Context = NotifyContext.UpdateTask,
                        Content = $"{accountName} {contentTemplate} <b>{taskCode}</b>",
                        HasViewed = false,
                        RefData = taskCode
                    });
                }
            }
        }

        /// <summary>
        /// add new notification to all member of project when there is someone create new task
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="taskids"></param>
        /// <param name="assignee"></param>
        /// <returns></returns>
        public async Task AssignTaskNotify(string accountName,
            int[] taskids, int assignee, string contentTemplate)
        {
            foreach(var id in taskids)
            {
                var taskCode = await _task.GetTaskCode(id);
                await AddNotification(new NotificationModel
                {
                    Account = new AccountModel { AccountId = assignee },
                    Context = NotifyContext.AssignTask,
                    Content = $"{accountName} {contentTemplate} (<b>{taskCode.Data}</b>)",
                    HasViewed = false,
                    RefData = taskCode.Data
                });
            }                   
        }

        /// <summary>
        /// add new notification to all member of project when manager deletes task
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="taskids"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task DeleteTaskNotify(string accountName,int accountId,
            int[] taskids, int projectId, string contentTemplate)
        {
            var memberIds = await _project.GetAccountIdList(projectId);
            if (memberIds.Any())
            {
                foreach (var id in memberIds)
                {
                    if (id == accountId)
                        continue;

                    foreach (var taskId in taskids)
                    {
                        var taskCode = await _task.GetTaskCode(id);
                        await AddNotification(new NotificationModel
                        {
                            Account = new AccountModel { AccountId = id },
                            Context = NotifyContext.DeleteTask,
                            Content = $"{accountName} {contentTemplate} (<b>{taskCode.Data}</b>)",
                            HasViewed = false,
                            RefData = taskCode.Data
                        });
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Add new notification
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<ResultModel<NotificationModel>> AddNotification(NotificationModel model)
        {
            ResultModel<NotificationModel> result = new ResultModel<NotificationModel>();
            var entity = model.ToEntity();
            entity.CreatedDate = model.CreatedDate = DateTime.Now;
            entity.Account = null;

            UnitOfWork.NotifyRepository.Add(entity);

            var iresult = await UnitOfWork.CommitAsync();
            if(iresult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = model;
            }
            return result;
        }

        /// <summary>
        /// Update notify status
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> UpdateNotifyStatus(int accountId)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var data = await UnitOfWork.NotifyRepository.GetManyAsync(c => c.AccountId == accountId 
                && c.HasViewed == false);

            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.HasViewed = true;
                    UnitOfWork.NotifyRepository.Update(item);
                }
                var iresult = await UnitOfWork.CommitAsync();
                result.Data = iresult > 0;
                result.Status = ActionStatus.Ok;
            }
            return result;
        }

        /// <summary>
        /// Delete notify item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> DeleteNotify(int id)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            UnitOfWork.NotifyRepository.Delete(id);
            var iresult = await UnitOfWork.CommitAsync();
            if(iresult > 0)
            {
                result.Data = true;
                result.Status = ActionStatus.Ok;
            }
            return result;
        }

        /// <summary>
        /// get the notification list is not yet viewed
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<IList<NotificationModel>>> NewNotificationList(int accountId)
        {
            ResultModel<IList<NotificationModel>> result = new ResultModel<IList<NotificationModel>>
            {
                Data = new List<NotificationModel>()
            };

            var data = await UnitOfWork
                .NotifyRepository
                .Entity
                .Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .Where(c => c.AccountId == accountId 
                         && c.HasViewed == false)
                .OrderByDescending(c => c.CreatedDate)
                .AsNoTracking()
                .ToListAsync();

            if (data.Any())
            {
                result.Data = data.ToModels();
                result.Status = ActionStatus.Ok;
            }
            return result;
        }

        /// <summary>
        /// get the notification list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<IList<IGrouping<DateTime, NotificationModel>>>> GetNotificationList(int accountId)
        {
            ResultModel<IList<IGrouping<DateTime, NotificationModel>>> result =
                                new ResultModel<IList<IGrouping<DateTime, NotificationModel>>> {
                    Data = new List<IGrouping<DateTime, NotificationModel>>()
             };

            var data = await UnitOfWork.NotifyRepository.Entity.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .Where(c => c.AccountId == accountId)
                .OrderByDescending(c=>c.CreatedDate)
                .AsNoTracking()
                .ToListAsync();

            if (data.Any())
            {
                result.Data = data.ToModels().GroupBy(t=>t.CreatedDate.Date).ToList();
                result.Status = ActionStatus.Ok;
            }
            return result;
        }

        /// <summary>
        /// count notification 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<int> CountNotification(int accountId)
        {
            var data = await UnitOfWork.NotifyRepository.Entity
                .CountAsync(c => c.AccountId == accountId);
      
            return data;
        }
    }
}
