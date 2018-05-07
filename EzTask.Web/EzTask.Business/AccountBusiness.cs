﻿using EzTask.DataAccess;
using EzTask.Framework.Security;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using System.IO;
using ImageStream = EzTask.Framework.IO.Stream;
using EzTask.Framework.ImageHandler;

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
        public async Task<Account> RegisterNew(Account account)
        {
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
                return account;
            }
            return null;
        }

        /// <summary>
        /// Update account information
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<AccountInfo> UpdateAccount(AccountInfo account)
        {
            var accountInfo = DbContext.AccountInfos.FirstOrDefault(c => c.Id == account.Id);
            if (accountInfo != null)
            {
                accountInfo.Update(account);
                
                var updateRecord = await DbContext.SaveChangesAsync();
                if (updateRecord > 0)
                {
                    return accountInfo;
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
        public async Task<AccountInfo> GetAccountInfo(int accountId)
        {
            return await DbContext.AccountInfos.Include(c => c.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(c=>c.AccountId == accountId);
        }

        /// <summary>
        /// Get account by account name and password
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Account> GetAccount(string accountName, string password)
        {
            return await DbContext.Accounts.Include(c=>c.AccountInfo).
                FirstOrDefaultAsync(c => c.AccountName == accountName 
                && c.Password == password);
        }

        /// <summary>
        /// Doing Login
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Account> Login(string accountName, string password)
        {
            var hash = Cryptography.GetHashString(accountName);
            password = Encrypt.Do(password, hash);
            return await GetAccount(accountName, password);
        }

        /// <summary>
        /// Get account by account name
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<Account> GetAccount(string accountName)
        {
            return await DbContext.Accounts.
                FirstOrDefaultAsync(c => c.AccountName == accountName);
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
        public async Task<IEnumerable<Account>> GetAccountList(int page, int pageSize,
            int manageUserId)
        {
           return await DbContext.Accounts.Where(c=> c.ManageAccountId == manageUserId)
                .Skip(pageSize * page - pageSize).Take(pageSize).ToListAsync();
        }
    }
}