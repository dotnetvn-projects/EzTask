using EzTask.Framework.Infrastructures;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Models.Notification;
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
        public NotificationBusiness(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Add new notification
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<NotificationModel>> AddNotify(NotificationModel model)
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
        /// get the notification list is not yet view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<NotificationModel>>> NewNotificationList(int accountId)
        {
            ResultModel<IEnumerable<NotificationModel>> result = new ResultModel<IEnumerable<NotificationModel>>();
            var data = await UnitOfWork.NotifyRepository.Entity.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .Where(c => c.AccountId == accountId && c.HasViewed == false).ToListAsync();

            return result;
        }
    }
}
