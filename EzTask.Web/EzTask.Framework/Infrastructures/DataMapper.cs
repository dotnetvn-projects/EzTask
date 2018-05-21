using AutoMapper;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using EzTask.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask
{
    public static class DataMapper
    {
        private static IMapper _mapper;
        public static void Config(IMapper mapper)
        {
            _mapper = mapper;
        }

        #region Account Mapper

        public static Account ToEntity(this RegisterModel model)
        {
            return _mapper.Map<Account>(model);
        }

        public static AccountModel ToModel(this Account entity)
        {
            return _mapper.Map<AccountModel>(entity);
        }

        public static IEnumerable<AccountModel> ToModels(this IEnumerable<Account> entity)
        {
            return _mapper.Map<IEnumerable<AccountModel>>(entity);
        }

        public static AccountInfoModel ToModel(this AccountInfo entity)
        {
            if (entity == null)
                return new AccountInfoModel();

            return _mapper.Map<AccountInfoModel>(entity);
        }

        public static AccountInfo ToEntity(this AccountInfoModel model)
        {
            return _mapper.Map<AccountInfo>(model);
        }
        #endregion

        #region Project

        public static ProjectModel ToModel(this Project entity)
        {
            if (entity == null)
                return null;

            return _mapper.Map<ProjectModel>(entity);
        }

        public static Project ToEntity(this ProjectModel model)
        {
            if (model == null)
                return null;

            return _mapper.Map<Project>(model);
        }

        public static IEnumerable<ProjectModel> ToModels(this IEnumerable<Project> entity)
        {
            var data = _mapper.Map<IEnumerable<ProjectModel>>(entity);
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

            return _mapper.Map<PhraseModel>(entity);
        }

        public static Phrase ToEntity(this PhraseModel model)
        {
            if (model == null)
                return null;

            return _mapper.Map<Phrase>(model);
        }

        public static IEnumerable<PhraseModel> ToModels(this IEnumerable<Phrase> entity)
        {
            return _mapper.Map<IEnumerable<PhraseModel>>(entity);
        }
        #endregion
    }

}
