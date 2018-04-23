using AutoMapper;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using EzTask.Web.Models.Account;
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




        #endregion
    }
}
