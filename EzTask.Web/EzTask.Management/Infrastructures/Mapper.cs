using EzTask.Entity;
using EzTask.Management.Models;
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
        #endregion
    }
}
