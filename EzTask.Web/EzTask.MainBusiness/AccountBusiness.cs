using EzTask.DataAccess;
using EzTask.Framework.Security;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;

namespace EzTask.MainBusiness
{
    public class AccountBusiness : BaseBusiness
    {
        public AccountBusiness(EzTaskDbContext dbContext):base(dbContext) {}
    
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
            account.Password = Encrypt.EncryptString(account.Password, 
                account.PasswordHash);
            account.UpdatedDate = DateTime.Now;
            account.AccountStatus = (int)AccountStatus.Active;

            EzTaskDbContext.Accounts.Add(account);
            await EzTaskDbContext.SaveChangesAsync();

            return account;
        }

        /// <summary>
        /// Update account information
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<AccountInfo> UpdateAccount(AccountInfo account)
        {
            var accountInfo = EzTaskDbContext.AccountInfos.FirstOrDefault(c => c.Id == account.Id);
            if (accountInfo != null)
            {
                accountInfo.Update(account);
                
                var updateRecord = await EzTaskDbContext.SaveChangesAsync();
                if (updateRecord > 0)
                {
                    return accountInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// Get account information
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AccountInfo> GetAccountInfo(int accountId)
        {
            return await EzTaskDbContext.AccountInfos.Include(c => c.Account)
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
            return await EzTaskDbContext.Accounts.
                FirstOrDefaultAsync(c => c.AccountName == accountName 
                && c.Password == password);
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
           return await EzTaskDbContext.Accounts.Where(c=> c.ManageAccountId == manageUserId)
                .Skip(pageSize * page - pageSize).Take(pageSize).ToListAsync();
        }
    }
}
