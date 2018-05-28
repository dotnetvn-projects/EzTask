﻿using AutoMapper;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using EzTask.Framework.Common;
using EzTask.Models;

namespace EzTask.Framework.Infrastructures
{
    public class DataMaperProfile : Profile
    {
        public DataMaperProfile()
        {
            AccountMaper();
            ProjectMapper();
            PhraseMapper();
            TaskMapper();
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
                .ForMember(c => c.AccountId, t => t.MapFrom(z => z.Id))
                .ForMember(c => c.AccountStatus, t => t.MapFrom(z => z.AccountStatus.ToEnum<ProjectStatus>()));

            ///Map AccountInfo entity to AccountInfoModel
            CreateMap<AccountInfo, AccountInfoModel>()
                .ForMember(c => c.AccountInfoId, t => t.MapFrom(z => z.Id))
                .ForMember(c => c.AccountName, t => t.MapFrom(z => z.Account.AccountName))
                .ForMember(c => c.Password, t => t.MapFrom(z => z.Account.Password));

            ///Map AccountInfoModel to AccountInfo entity
            CreateMap<AccountInfoModel, AccountInfo>()
                .ForMember(c => c.Id, t => t.MapFrom(z => z.AccountInfoId));
        }

        private void ProjectMapper()
        {
            //Map Project entity to ProjectModel
            CreateMap<Project, ProjectModel>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToEnum<ProjectStatus>()))
                .ForMember(c => c.ProjectId, t => t.MapFrom(z => z.Id))
                .ForPath(c => c.Owner.AccountId, t => t.MapFrom(z => z.Account != null ? z.Account.Id : 0))
                .ForPath(c => c.Owner.AccountName, t => t.MapFrom(z => z.Account != null ? z.Account.AccountName : string.Empty))
                .ForPath(c => c.Owner.DisplayName, t => t.MapFrom(z =>
                                (z.Account != null && z.Account.AccountInfo != null) ?
                                        z.Account.AccountInfo.DisplayName : string.Empty))
                .ForPath(c => c.Owner.FullName, t => t.MapFrom(z =>
                                (z.Account != null && z.Account.AccountInfo != null) ?
                                        z.Account.AccountInfo.FullName : string.Empty))
                .ForMember(c => c.Color, t => t.Ignore())
                .ForMember(c => c.BoxType, t => t.Ignore());

            //Map ProjectModel to Project entity
            CreateMap<ProjectModel, Project>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToInt16<ProjectStatus>()))
                .ForMember(c => c.Owner, t => t.MapFrom(z => z.Owner.AccountId))
                .ForMember(c => c.Id, t => t.MapFrom(z => z.ProjectId))
                .ForMember(c => c.Account, t => t.Ignore());
        }

        private void PhraseMapper()
        {
            //Map phrase entity to phrase model
            CreateMap<Phrase, PhraseModel>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToEnum<PhraseStatus>()));

            //Map phrase model to phrase entity
            CreateMap<PhraseModel, Phrase>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToInt16<PhraseStatus>()));
        }

        private void TaskMapper()
        {
            //Map TaskItem entity to TaskItem model
            CreateMap<TaskItem, TaskItemModel>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToEnum<TaskStatus>()))
                .ForMember(c => c.Priority, t => t.MapFrom(z => z.Priority.ToEnum<TaskStatus>()))
                .ForMember(c => c.TaskId, t => t.MapFrom(z => z.Id));

            //Map TaskItem model to TaskItem entity
            CreateMap<TaskItemModel, TaskItem>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToInt16<TaskStatus>()))
                .ForMember(c => c.Priority, t => t.MapFrom(z => z.Priority.ToInt16<TaskStatus>()))
                .ForMember(c => c.Id, t => t.MapFrom(z => z.TaskId));
        }
    }
}
