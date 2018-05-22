using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using EzTask.Models;
using System.Collections.Generic;

namespace EzTask.Framework.Infrastructures
{
    public static class DataMapper
    {        
        #region Account Mapper

        public static Account ToEntity(this RegisterModel model)
        {
            return FrameworkInitializer.Mapper.Map<Account>(model);
        }

        public static AccountModel ToModel(this Account entity)
        {
            return FrameworkInitializer.Mapper.Map<AccountModel>(entity);
        }

        public static IEnumerable<AccountModel> ToModels(this IEnumerable<Account> entity)
        {
            return FrameworkInitializer.Mapper.Map<IEnumerable<AccountModel>>(entity);
        }

        public static AccountInfoModel ToModel(this AccountInfo entity)
        {
            if (entity == null)
                return new AccountInfoModel();

            return FrameworkInitializer.Mapper.Map<AccountInfoModel>(entity);
        }

        public static AccountInfo ToEntity(this AccountInfoModel model)
        {
            return FrameworkInitializer.Mapper.Map<AccountInfo>(model);
        }
        #endregion

        #region Project

        public static ProjectModel ToModel(this Project entity)
        {
            if (entity == null)
                return null;

            return FrameworkInitializer.Mapper.Map<ProjectModel>(entity);
        }

        public static Project ToEntity(this ProjectModel model)
        {
            if (model == null)
                return null;

            return FrameworkInitializer.Mapper.Map<Project>(model);
        }

        public static IEnumerable<ProjectModel> ToModels(this IEnumerable<Project> entity)
        {
            var data = FrameworkInitializer.Mapper.Map<IEnumerable<ProjectModel>>(entity);
            if (data == null)
                return data;

            foreach (var item in data)
            {
                switch (item.Status)
                {
                    case ProjectStatus.Pending:
                        item.BoxType = "primary";
                        item.Color = "light-blue";
                        break;
                    case ProjectStatus.Completed:
                        item.BoxType = "success";
                        item.Color = "green";
                        break;
                    case ProjectStatus.Canceled:
                        item.BoxType = "warning";
                        item.Color = "yellow";
                        break;
                    case ProjectStatus.Failed:
                        item.BoxType = "danger";
                        item.Color = "red";
                        break;
                    case ProjectStatus.Implementing:
                        item.BoxType = "info fix-box-info";
                        item.Color = "info-color";
                        break;
                }
            }
            return data;
        }

        #endregion

        #region Phrase Mapper
        public static PhraseModel ToModel(this Phrase entity)
        {
            if (entity == null)
                return null;

            return FrameworkInitializer.Mapper.Map<PhraseModel>(entity);
        }

        public static Phrase ToEntity(this PhraseModel model)
        {
            if (model == null)
                return null;

            return FrameworkInitializer.Mapper.Map<Phrase>(model);
        }

        public static IEnumerable<PhraseModel> ToModels(this IEnumerable<Phrase> entity)
        {
            return FrameworkInitializer.Mapper.Map<IEnumerable<PhraseModel>>(entity);
        }
        #endregion
    }

}
