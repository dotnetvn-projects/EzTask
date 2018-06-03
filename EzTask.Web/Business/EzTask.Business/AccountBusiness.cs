using EzTask.DataAccess;
using EzTask.Framework.Security;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using ImageStream = EzTask.Framework.IO.Stream;
using EzTask.Framework.ImageHandler;
using EzTask.Models;
using EzTask.Framework.Infrastructures;
using EzTask.Models.Enum;
using EzTask.Repository;
using EzTask.Entity.Data;
using EzTask.Interfaces;

namespace EzTask.Business
{
    public class AccountBusiness : BaseBusiness<EzTaskDbContext>
    {
        private readonly ImageProcessor _imageProcessor;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<AccountInfo> _accountInfoRepository;

        public AccountBusiness(
           ImageProcessor imageProcessor,
           IRepository<Account> accountRepository,
           IRepository<AccountInfo> accountInfoRepository)
        { 
            _imageProcessor = imageProcessor;
            _accountRepository = accountRepository;
            _accountInfoRepository = accountInfoRepository;
        }
    
        /// <summary>
        /// Register a new account to EzTask
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<ResultModel<AccountModel>> RegisterNew(RegisterModel model)
        {
            ResultModel<AccountModel> result = new ResultModel<AccountModel>
            {
                Status = ActionStatus.Failed
            };

            var account = model.ToEntity();
            if (account.Id < 1)
                account.CreatedDate = DateTime.Now;

            account.PasswordHash = Cryptography.GetHashString(account.AccountName);
            account.Password = Encrypt.Do(account.Password, 
                account.PasswordHash);

            account.UpdatedDate = DateTime.Now;
            account.AccountStatus = (int)AccountStatus.Active;

            _accountRepository.Add(account);
            var insertedRecord = await UnitOfWork.CommitAsync();

            if(insertedRecord > 0)
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

            var accountInfo = await _accountInfoRepository.GetAsync(c => c.Id == model.AccountInfoId);
            if (accountInfo != null)
            {
                accountInfo.Update(accountInfo);
                
                var updateRecord = await UnitOfWork.CommitAsync();
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

            var accountInfo = await _accountInfoRepository.GetAsync(c => c.AccountId == accountId);
            if (accountInfo != null)
            {
                var bytes = await ImageStream.ConvertStreamToBytes(stream);
                var imageData = await _imageProcessor.CreateNewSize(bytes, 250, 250);
                stream.Dispose();

                accountInfo.DisplayImage = imageData;

                var iResult = await UnitOfWork.CommitAsync();
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
            var data = await _accountInfoRepository.Entity.Include(c => c.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(c=>c.AccountId == accountId);

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
            var accountInfo = await _accountInfoRepository.Entity.Include(c=>c.Account).
                FirstOrDefaultAsync(c => c.Account.AccountName == accountName 
                && c.Account.Password == password);

            return accountInfo.ToModel();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> Login(LoginModel model)
        {
            var hash = Cryptography.GetHashString(model.AccountName);
            model.Password = Encrypt.Do(model.Password, hash);
            var account = await GetAccount(model.AccountName, model.Password);
            return account;
        }

        /// <summary>
        /// Get account by account name
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<AccountModel> GetAccount(string accountName)
        {
            var data = await _accountRepository.GetAsync(c => c.AccountName == accountName);
            return data.ToModel();
        }

        public async Task<byte[]> LoadAvatar(int accountId)
        {
            var data = await _accountInfoRepository.GetAsync(c => 
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
           var data = await _accountRepository.Entity.Where(c=> c.ManageAccountId == manageUserId)
                .Skip(pageSize * page - pageSize).Take(pageSize).ToListAsync();
            return data.ToModels();
        }
    }
}
