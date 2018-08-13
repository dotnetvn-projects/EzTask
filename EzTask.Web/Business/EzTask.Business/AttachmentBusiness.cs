using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EzTask.Framework.Infrastructures;
using System.IO;
using FileStream = EzTask.Framework.IO.Stream;
using File = EzTask.Framework.IO.File;
using EzTask.Interfaces;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Repository;
using EzTask.Entity.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EzTask.Business
{
    public class AttachmentBusiness : BusinessCore
    {
        private readonly IWebHostEnvironment _host;
        public AttachmentBusiness(UnitOfWork unitOfWork, IWebHostEnvironment host) : base(unitOfWork)
        {
            _host = host;
        }

        /// <summary>
        /// Get attachment list
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AttachmentModel>> GetAttachments(int accountId)
        {
            var iResult = await UnitOfWork.AttachRepository.Entity.Include(c=>c.Task).
                Include(c=>c.User).ThenInclude(c=>c.AccountInfo).AsNoTracking()
                .Where(c => c.AddedUser == accountId).OrderByDescending(c => c.AddedDate)
                .ToListAsync();

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

            if(entity.Id <= 0)
            {
                entity.AddedDate = DateTime.Now;
            }

            UnitOfWork.AttachRepository.Add(entity);
            var iResult = await UnitOfWork.CommitAsync();

            if(iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = entity.ToModel();
            }

            return result;
        }
    }
}
