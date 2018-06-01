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

namespace EzTask.Business
{
    public class AccountBusiness : BaseBusiness
    {
        private ImageProcessor _imageProcessor;
        public AccountBusiness(EzTaskDbContext dbContext, 
            ImageProcessor imageProcessor):base(dbContext)
        {
            _imageProcessor = imageProcessor;
        }
    
        /// <summary>
        /// Register a new account to EzTask
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<AccountModel> RegisterNew(RegisterModel model)
        {
            var account = model.ToEntity();
            if (account.Id < 1)
                account.CreatedDate = DateTime.Now;

            account.PasswordHash = Cryptography.GetHashString(account.AccountName);
            account.Password = Encrypt.Do(account.Password, 
                account.PasswordHash);

            account.UpdatedDate = DateTime.Now;
            account.AccountStatus = (int)AccountStatus.Active;

            DbContext.Accounts.Add(account);
            var insertedRecord = await DbContext.SaveChangesAsync();

            if(insertedRecord > 0)
            {
                return account.ToModel();
            }
            return null;
        }

        /// <summary>
        /// Update account information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> UpdateAccount(AccountInfoModel model)
        {
            var accountInfo = DbContext.AccountInfos.FirstOrDefault(c => c.Id == model.AccountInfoId);
            if (accountInfo != null)
            {
                accountInfo.Update(accountInfo);
                
                var updateRecord = await DbContext.SaveChangesAsync();
                if (updateRecord > 0)
                {
                    return accountInfo.ToModel();
                }
            }
            return null;
        }

        /// <summary>
        /// Update avatar
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAvatar(int accountId, Stream stream)
        {
            var accountInfo = DbContext.AccountInfos.FirstOrDefault(c => c.AccountId == accountId);
            if (accountInfo != null)
            {
                var bytes = await ImageStream.ConvertStreamToBytes(stream);
                var imageData = await _imageProcessor.CreateNewSize(bytes, 250, 250);
                stream.Dispose();

                accountInfo.DisplayImage = imageData;

                var iResult = await DbContext.SaveChangesAsync();
                if (iResult > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get account information
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> GetAccountInfo(int accountId)
        {
            var data = await DbContext.AccountInfos.Include(c => c.Account)
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
            var accountInfo = await DbContext.AccountInfos.Include(c=>c.Account).
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
            var data = await DbContext.Accounts.
                FirstOrDefaultAsync(c => c.AccountName == accountName);
            return data.ToModel();
        }

        public async Task<byte[]> LoadAvatar(int accountId)
        {
            var data = await DbContext.AccountInfos
                     .AsNoTracking()
                         .FirstOrDefaultAsync(c => c.AccountId == accountId);

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
           var data = await DbContext.Accounts.Where(c=> c.ManageAccountId == manageUserId)
                .Skip(pageSize * page - pageSize).Take(pageSize).ToListAsync();
            return data.ToModels();
        }
    }
}
