using EzTask.Framework.Common;
using EzTask.Framework.Infrastructures;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Entity.Data;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class NotificationBusiness : BusinessCore
    {
        private readonly ProjectBusiness _project;
        private readonly AccountBusiness _account;
        private readonly TaskBusiness _task;
        private const string TASK_DETAIL_REF = "<a href='/task/{0}.html'>{1}</a>";

        public NotificationBusiness(UnitOfWork unitOfWork,
            ProjectBusiness project, AccountBusiness account, TaskBusiness task) : base(unitOfWork)
        {
            _project = project;
            _account = account;
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
                        Content = $"{accountName} {contentTemplate} ({string.Format(TASK_DETAIL_REF, taskCode, taskCode)})",
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
                        Content = $"{accountName} {contentTemplate} ({string.Format(TASK_DETAIL_REF, taskCode, taskCode)})",
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
                    Content = $"{accountName} {contentTemplate} ({string.Format(TASK_DETAIL_REF, taskCode.Data, taskCode.Data)})",
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
                            Content = $"{accountName} {contentTemplate} ({taskCode.Data})",
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
        /// get the notification list is not yet viewed
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<NotificationModel>>> NewNotificationList(int accountId)
        {
            ResultModel<IEnumerable<NotificationModel>> result = new ResultModel<IEnumerable<NotificationModel>>();

            var data = await UnitOfWork.NotifyRepository.Entity.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .Where(c => c.AccountId == accountId 
                         && c.HasViewed == false
                         && c.Context != (short)NotifyContext.Message)
                .OrderByDescending(c => c.CreatedDate)
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
        public async Task<ResultModel<IEnumerable<NotificationModel>>> GetNotificationList(int accountId,
            NotifyContext context)
        {
            ResultModel<IEnumerable<NotificationModel>> result = new ResultModel<IEnumerable<NotificationModel>>();
            var notifyContext = context.ToInt16<NotifyContext>();

            var data = await UnitOfWork.NotifyRepository.Entity.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .Where(c => c.AccountId == accountId
                         && c.Context == notifyContext)
                .OrderByDescending(c=>c.CreatedDate)
                .ToListAsync();

            if (data.Any())
            {
                result.Data = data.ToModels();
                result.Status = ActionStatus.Ok;
            }
            return result;
        }
    }
}
