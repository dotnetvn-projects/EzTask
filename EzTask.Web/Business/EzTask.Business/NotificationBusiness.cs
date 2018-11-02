using EzTask.Framework.Common;
using EzTask.Framework.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Models;
using EzTask.Models.Enum;
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
        private const string TASK_DETAIL_REF = "<a href='/task/{0}.html'>{1}</a>";

        public NotificationBusiness(UnitOfWork unitOfWork,
            ProjectBusiness project, AccountBusiness account) : base(unitOfWork)
        {
            _project = project;
            _account = account;
        }


        /// <summary>
        /// add new notification to all member of project 
        /// when there is someone do some action on task
        /// </summary>
        /// <param name="member"></param>
        /// <param name="taskCode"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task AddTaskNotify(CurrentAccount account, 
            string taskCode, int projectId, NotifyContext context,  string contentTemplate)
        {
            var memberIds = await _project.GetAccountIdList(projectId);
            if (memberIds.Any())
            {
                foreach(var id in memberIds)
                {
                    if (id == account.AccountId)
                        continue;

                    await AddNotification(new NotificationModel
                    {
                        Account = new AccountModel { AccountId = id },
                        Context = context,
                        Content = $"{account.DisplayName} {contentTemplate}",
                        HasViewed = false,
                        RefData = string.Format(TASK_DETAIL_REF, taskCode, taskCode)
                    });
                }
            }         
        }

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
            ResultModel<IEnumerable<NotificationModel>> result = new ResultModel<IEnumerable<NotificationModel>>
            {
                Data = new List<NotificationModel>()
            };

            var notifyContext = (short)NotifyContext.Message;

            var data = await UnitOfWork.NotifyRepository.Entity
                .AsNoTracking()
                .Where(c => c.AccountId == accountId 
                         && c.HasViewed == false
                         && c.Context != notifyContext)
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
        public async Task<ResultModel<IEnumerable<NotificationModel>>> GetNotificationList(int accountId)
        {
            ResultModel<IEnumerable<NotificationModel>> result = new ResultModel<IEnumerable<NotificationModel>>
            {
                Data = new List<NotificationModel>()
            };

            var data = await UnitOfWork.NotifyRepository.Entity.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .Where(c => c.AccountId == accountId)
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
