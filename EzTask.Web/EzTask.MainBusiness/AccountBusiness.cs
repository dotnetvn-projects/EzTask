using EzTask.DataAccess;
using EzTask.Entity;
using EzTask.Framework.Security;
using EzTask.Framework.Enum;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                if (account.Id < 1)
                    account.CreatedDate = DateTime.Now;

                account.PasswordHash = Cryptography.GetHashString(account.AccountName);
                account.Password = Encrypt.EncryptString(account.Password, 
                    account.PasswordHash);
                account.UpdatedDate = DateTime.Now;
                account.AccountStatus = (int)AccountStatus.None;

                EzTaskDbContext.Accounts.Add(account);
                await EzTaskDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                //TODO log
            }

            return account;
        }

        //public async Task<Account> UpdateAccount(Account account)
        //{

        //}

        /// <summary>
        /// Get account information
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AccountInfo> GetAccountInfo(int accountId)
        {
            return await EzTaskDbContext.AccountInfos.
                FirstOrDefaultAsync(c=>c.AccountId == accountId);
        }

        public async Task<Account> GetAccount(string accountName, string password)
        {

            //TODO get by account name
            //Calulate password to get hash before comparing
            return await EzTaskDbContext.Accounts.
                FirstOrDefaultAsync(c => c.AccountName == accountName 
                && c.Password == password);
        }
    }
}
