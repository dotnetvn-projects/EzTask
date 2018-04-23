using EzTask.Entity.Data;
using EzTask.Web.Models.Account;
using EzTask.Framework.Common;
using EzTask.Entity.Framework;
using AutoMapper;

namespace EzTask.Web.Infrastructures
{
    public  class EzTaskMapperProfile :Profile
    {
        public EzTaskMapperProfile()
        {
            AccountMaper();
        }

        private void AccountMaper()
        {
            //Map RegisterModel to Account Entity
            CreateMap<RegisterModel, Account>()
                .ForMember(c => c.Id, t => t.MapFrom(z => z.AccountId))
                .ForPath(c => c.AccountInfo.AccountId, t => t.MapFrom(z => z.AccountId))
                .ForPath(c => c.AccountInfo.Email, t => t.MapFrom(z => z.AccountName))
                .ForPath(c => c.AccountInfo.FullName, t => t.MapFrom(z => z.FullName))
                .ForPath(c => c.AccountInfo.DisplayName, t => t.MapFrom(z => z.DisplayName));

            ///Map Account entity to Account Model
            CreateMap<Account, AccountModel>()
                .ForMember(c => c.AccountId, t => t.MapFrom(z => z.Id));


            ///Map AccountInfo entity to AccountInfoModel
            CreateMap<AccountInfo, AccountInfoModel>()
                .ForMember(c => c.AccountInfoId, t => t.MapFrom(z => z.Id))
                .ForMember(c => c.AccountName, t => t.MapFrom(z => z.Account.AccountName))
                .ForMember(c => c.Password, t => t.MapFrom(z => z.Account.Password));

            ///Map AccountInfoModel to AccountInfo entity
            CreateMap<AccountInfoModel, AccountInfo>()
                .ForMember(c => c.Id, t => t.MapFrom(z => z.AccountInfoId));
        }       
    }
}
