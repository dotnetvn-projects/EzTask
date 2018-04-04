using EzTask.Entity.Data;
using EzTask.Management.Models.Account;
using EzTask.Management.Models.Project;
using System;
using System.Collections.Generic;
using EzTask.Framework.Common;
using EzTask.Entity.Framework;

namespace EzTask.Management.Infrastructures
{
    public static class Mapper
    {
        #region Account Mapper

        public static Account MapToEntity(this RegisterModel model)
        {
            return new Account
            {
               AccountName = model.AccountName,
               Password = model.Password,
               Id = model.AccountId,
               AccountInfo =new AccountInfo
               {
                 AccountId = model.AccountId,
                 Email = model.AccountName,
                 FullName = model.FullName,
                 DisplayName = model.DisplayName
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

        public static AccountInfoModel MapToModel(this AccountInfo entity)
        {
            return new AccountInfoModel
            {
                AccountInfoId = entity.Id,
                JobTitle = entity.JobTitle,
                AccountId = entity.AccountId,
                AccountName = entity.Account.AccountName,
                Password = entity.Account.Password,
                Address1 = entity.Address1,
                Address2 = entity.Address2,
                BirthDay = entity.BirthDay,
                Comment = entity.Comment,
                DisplayImage = entity.DisplayImage,
                DisplayName = entity.DisplayName,
                Document = entity.Document,
                Email = entity.Email,
                FullName = entity.FullName,
                Introduce = entity.Introduce,
                PhoneNumber = entity.PhoneNumber
            };
        }

        public static AccountInfo MapToEntity(this AccountInfoModel model)
        {
            return new AccountInfo
            {
                Id = model.AccountInfoId,
                JobTitle = model.JobTitle,
                AccountId = model.AccountId,
                Address1 = model.Address1,
                Address2 = model.Address2,
                BirthDay = model.BirthDay,
                Comment = model.Comment,
                DisplayImage = model.DisplayImage,
                DisplayName = model.DisplayName,
                Document = model.Document,
                Email = model.Email,
                FullName = model.FullName,
                Introduce = model.Introduce,
                PhoneNumber = model.PhoneNumber
            };
        }
        #endregion

        #region Project

        public static ProjectModel MapToModel(this Project entity)
        {
            if (entity == null)
                return null;

            var data = new ProjectModel
            {
                Comment = entity.Comment,
                CreatedDate = entity.CreatedDate,
                Description = entity.Description,
                MaximumUser = entity.MaximumUser,
                ProjectCode = entity.ProjectCode,
                ProjectId = entity.Id,
                ProjectName = entity.ProjectName,
                Status = entity.Status.ToEnum<ProjectStatus>(),
                UpdatedDate = entity.UpdatedDate
            };

            if(entity.Account !=null)
            {
                data.Owner = new AccountModel
                {
                  AccountId = entity.Id,
                  AccountName = entity.Account.AccountName
                };

                if(entity.Account.AccountInfo !=null)
                {
                    data.Owner.DisplayName = entity.Account.AccountInfo.DisplayName;
                    data.Owner.FullName = entity.Account.AccountInfo.FullName;
                }
            }

            return data;
        }

        public static Project MapToEntity(this ProjectModel model)
        {
            if (model == null)
                return null;

            return new Project
            {
                Comment = model.Comment,
                CreatedDate = model.CreatedDate,
                Description = model.Description,
                Id = model.ProjectId,
                MaximumUser = model.MaximumUser,
                ProjectCode = model.ProjectCode,
                ProjectName = model.ProjectName,
                Status = model.Status.ToInt16<ProjectStatus>(),
                Owner = model.Owner.AccountId,
                UpdatedDate = model.UpdatedDate
            };
        }

        #endregion
    }
}
