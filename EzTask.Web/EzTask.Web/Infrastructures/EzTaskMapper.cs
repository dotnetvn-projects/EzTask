using AutoMapper;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using EzTask.Web.Models.Account;
using EzTask.Web.Models.Project;
using System.Collections.Generic;

namespace EzTask.Web.Infrastructures
{
    public static class EzTaskMapper
    {
        private static IMapper _mapper;
        public static void Config(IMapper mapper)
        {
            _mapper = mapper;
        }

        #region Account Mapper

        public static Account MapToEntity(this RegisterModel model)
        {
           return _mapper.Map<Account>(model);
        }

        public static AccountModel MapToModel(this Account entity)
        {
            return _mapper.Map<AccountModel>(entity);
        }

        public static IEnumerable<AccountModel> MapToModels(this IEnumerable<Account> entity)
        {
            return _mapper.Map<IEnumerable<AccountModel>>(entity);
        }

        public static AccountInfoModel MapToModel(this AccountInfo entity)
        {
            return _mapper.Map<AccountInfoModel>(entity);
        }

        public static AccountInfo MapToEntity(this AccountInfoModel model)
        {
            return _mapper.Map<AccountInfo>(model);
        }
        #endregion

        #region Project

        public static ProjectModel MapToModel(this Project entity)
        {
            if (entity == null)
                return null;

            return _mapper.Map<ProjectModel>(entity);
        }

        public static Project MapToEntity(this ProjectModel model)
        {
            if (model == null)
                return null;

            return _mapper.Map<Project>(model);
        }

        public static IEnumerable<ProjectModel> MapToModels(this IEnumerable<Project> entity)
        {
            var data = _mapper.Map<IEnumerable<ProjectModel>>(entity);
            if (data == null)
                return data;

            foreach(var item in data)
            {
                switch(item.Status)
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
    }
}
