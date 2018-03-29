using EzTask.Entity;
using EzTask.Management.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Management.Infrastructures
{
    public static class Mapper
    {
        #region Account Mapper
        public static Account MapToEntity(this AccountModel model)
        {
            return new Account
            {
               AccountName = model.AccountName,
               Password = model.Password,
               Id = model.AccountId,
               AccountInfo =new AccountInfo
               {
                 AccountId = model.AccountId,
                 FullName = model.FullName,
                 DisplayName = model.DisplayName,
               }
            };
        }

        public static AccountModel MapToModel(this Account entity)
        {
            return new AccountModel
            {
                AccountName = entity.AccountName,
                Password = entity.Password,
                AccountId = entity.Id
            };
        }

        public static IEnumerable<AccountModel> MapToModels(this IEnumerable<Account> model)
        {
            var accountList = new List<AccountModel>();
            foreach(var item in model)
            {
                accountList.Add(item.MapToModel());
            }
            return accountList;
        }
        #endregion
    }
}
