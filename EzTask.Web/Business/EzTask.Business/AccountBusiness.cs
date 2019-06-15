using EzTask.Framework.ImageHandler;
using EzTask.Framework.Infrastructures;
using EzTask.Framework.IO;
using EzTask.Framework.Security;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class AccountBusiness : BusinessCore
    {
        private readonly ImageProcessor _imageProcessor;

        public AccountBusiness(ImageProcessor imageProcessor,
           UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _imageProcessor = imageProcessor;
        }

        /// <summary>
        /// Register a new account to EzTask
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<ResultModel<AccountModel>> RegisterNew(AccountModel model)
        {
            ResultModel<AccountModel> result = new ResultModel<AccountModel>
            {
                Status = ActionStatus.Failed
            };

            var account = model.ToEntity();
            if (account.Id < 1)
            {
                account.CreatedDate = DateTime.Now;
            }

            account.PasswordHash = Cryptography.GetHashString(account.AccountName);
            account.Password = Encrypt.Do(account.Password,
                account.PasswordHash);

            account.UpdatedDate = DateTime.Now;
            account.AccountStatus = (int)AccountStatus.Active;

            UnitOfWork.AccountRepository.Add(account);
            int insertedRecord = await UnitOfWork.CommitAsync();

            if (insertedRecord > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = account.ToModel();
            }

            return result;
        }

        /// <summary>
        /// Update account information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<AccountInfoModel>> UpdateAccount(AccountInfoModel model)
        {
            ResultModel<AccountInfoModel> result = new ResultModel<AccountInfoModel>
            {
                Status = ActionStatus.NotFound
            };

            var accountInfo = await UnitOfWork.AccountInfoRepository
                .GetAsync(c => c.Id == model.AccountInfoId);

            if (accountInfo != null)
            {
                accountInfo.Update(accountInfo);

                int updateRecord = await UnitOfWork.CommitAsync();
                if (updateRecord > 0)
                {
                    result.Status = ActionStatus.Ok;
                    result.Data = accountInfo.ToModel();
                }
            }
            return result;
        }

        /// <summary>
        /// Update avatar
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> UpdateAvatar(int accountId, Stream stream)
        {
            ResultModel<bool> result = new ResultModel<bool>
            {
                Status = ActionStatus.Failed
            };

            var accountInfo = await UnitOfWork.AccountInfoRepository.GetAsync(c => c.AccountId == accountId);
            if (accountInfo != null)
            {
                byte[] bytes = await stream.ConvertStreamToBytes();
                byte[] imageData = await _imageProcessor.CreateNewSize(bytes, 250, 250);
                stream.Dispose();

                accountInfo.DisplayImage = imageData;

                int iResult = await UnitOfWork.CommitAsync();
                if (iResult > 0)
                {
                    result.Status = ActionStatus.Ok;
                    result.Data = true;
                }
            }
            else
            {
                result.Status = ActionStatus.NotFound;
            }
            return result;
        }

        /// <summary>
        /// Get account information
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> GetAccountInfo(int accountId)
        {
            var data = await UnitOfWork.AccountInfoRepository
                .Entity.Include(c => c.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.AccountId == accountId);

            return data.ToModel();
        }

        /// <summary>
        /// Get account by account name and password
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> GetAccount(string accountName, string password)
        {
            var accountInfo = await UnitOfWork.AccountInfoRepository
                .Entity.Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.Account.AccountName == accountName
                             && c.Account.Password == password);

            return accountInfo.ToModel();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> Login(AccountModel model)
        {
            string hash = Cryptography.GetHashString(model.AccountName);
            model.Password = Encrypt.Do(model.Password, hash);

            AccountInfoModel account = await GetAccount(model.AccountName, model.Password);
            return account;
        }

        /// <summary>
        /// Get account by account name
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<AccountModel> GetAccount(string accountName)
        {
            var data = await UnitOfWork.AccountRepository.GetAsync(c => c.AccountName == accountName);
            return data.ToModel();
        }

        /// <summary>
        /// Get account info by account name
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> GetAccountInfo(string accountName)
        {
            var accountInfo = await UnitOfWork.AccountInfoRepository
                        .Entity
                        .Include(c => c.Account)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Account.AccountName == accountName);

            return accountInfo.ToModel();
        }

        /// <summary>
        /// Load avatar
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<byte[]> LoadAvatar(int accountId)
        {
            var data = await UnitOfWork.AccountInfoRepository.GetAsync(c =>
                    c.AccountId == accountId, allowTracking: false);

            return data.DisplayImage;
        }

        /// <summary>
        /// Get list account
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="manageUserId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AccountModel>> GetAccountList(int page, int pageSize,
            int manageUserId)
        {
            var data = await UnitOfWork.AccountRepository
                .Entity
                .Where(c => c.ManageAccountId == manageUserId)
                .Skip(pageSize * page - pageSize)
                .Take(pageSize)
                .ToListAsync();
            return data.ToModels();
        }
    }
}