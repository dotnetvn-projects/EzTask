using AutoMapper;
using EzTask.Entity.Data;
using EzTask.Framework.Common;
using EzTask.Model;
using EzTask.Model.Enum;
using System.Linq;

namespace EzTask.Model
{
    public class DataMaperProfile : Profile
    {
        public DataMaperProfile()
        {
            AccountMaper();
            ProjectMapper();
            PhaseMapper();
            TaskMapper();
            NotificationMapper();
            TodoMapper();
        }

        private void AccountMaper()
        {
            //Map Account entity to Account Model
            CreateMap<Account, AccountModel>()
                .ForMember(c => c.AccountId, t => t.MapFrom(z => z.Id))
                .ForMember(c => c.AccountStatus, t => t.MapFrom(z => z.AccountStatus.ToEnum<AccountStatus>()))
                .ForMember(c => c.DisplayName, t => t.MapFrom(z => z.AccountInfo.DisplayName));

            //Map Account model to Account entity
            CreateMap<AccountModel, Account>()
                .ForMember(c => c.Id, t => t.MapFrom(z => z.AccountId))
                .ForMember(c => c.AccountStatus, t => t.MapFrom(z => z.AccountStatus.ToInt16<AccountStatus>()))
                .ForPath(c => c.AccountInfo.AccountId, t => t.MapFrom(z => z.AccountId))
                .ForPath(c => c.AccountInfo.Email, t => t.MapFrom(z => z.AccountName))
                .ForPath(c => c.AccountInfo.FullName, t => t.MapFrom(z => z.FullName))
                .ForPath(c => c.AccountInfo.DisplayName, t => t.MapFrom(z => z.DisplayName));

            //Map AccountInfo entity to AccountInfoModel
            CreateMap<AccountInfo, AccountInfoModel>()
                .ForMember(c => c.AccountInfoId, t => t.MapFrom(z => z.Id))
                .ForMember(c => c.CreatedDate, t => t.MapFrom(z => z.Account.CreatedDate))
                .ForMember(c => c.AccountName, t => t.MapFrom(z => z.Account.AccountName))
                .ForMember(c => c.Password, t => t.MapFrom(z => z.Account.Password))
                .ForMember(c => c.DisplayName, t => t.MapFrom(z => z.Account.AccountInfo != null?
                                                                z.Account.AccountInfo.DisplayName : string.Empty));

            //Map AccountInfoModel to AccountInfo entity
            CreateMap<AccountInfoModel, AccountInfo>()
                .ForMember(c => c.Id, t => t.MapFrom(z => z.AccountInfoId));

            //Map RecoverSession Model to Entity
            CreateMap<RecoverSessionModel, RecoverSession>();

            //Map Entitu  to RecoverSession Model
            CreateMap<RecoverSession, RecoverSessionModel>();
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

        private void PhaseMapper()
        {
            //Map phase entity to phase model
            CreateMap<Phase, PhaseModel>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToEnum<PhaseStatus>()));

            //Map phase model to phase entity
            CreateMap<PhaseModel, Phase>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToInt16<PhaseStatus>()));
        }

        private void TaskMapper()
        {
            //Map Attachment to Attachment model
            CreateMap<Attachment, AttachmentModel>()
                .ForMember(c => c.AttatchmentId, t => t.MapFrom(z => z.Id));

            //Map AttachmentModel to Attachment entity
            CreateMap<AttachmentModel, Attachment>()
                .ForMember(c => c.Id, t => t.MapFrom(z => z.AttatchmentId))
                .ForMember(c => c.TaskId, t => t.MapFrom(z => z.Task != null ? z.Task.TaskId : 0))
                .ForMember(c => c.AddedUser, t => t.MapFrom(z => z.User != null ? z.User.AccountId : 0));

            //Map history to history model
            CreateMap<TaskHistory, TaskHistoryModel>()
                .ForMember(c => c.HistoryId, t => t.MapFrom(z => z.Id));               

            //Map history model to history entity
            CreateMap<TaskHistoryModel, TaskHistory>()
                .ForMember(c => c.Id, t => t.MapFrom(z => z.HistoryId))
                .ForMember(c => c.TaskId, t => t.MapFrom(z => z.Task != null ? z.Task.TaskId : 0))
                .ForMember(c => c.UpdatedUser, t => t.MapFrom(z => z.User != null ? z.User.AccountId : 0));

            //Map TaskItem entity to TaskItem model
            CreateMap<TaskItem, TaskItemModel>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToEnum<TaskItemStatus>()))
                .ForMember(c => c.Priority, t => t.MapFrom(z => z.Priority.ToEnum<TaskItemStatus>()))
                .ForMember(c => c.TaskId, t => t.MapFrom(z => z.Id))
                .ForMember(c => c.HasAttachment, t => t.MapFrom(z => z.Attachments != null && z.Attachments.Any()));

            //Map TaskItem model to TaskItem entity
            CreateMap<TaskItemModel, TaskItem>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToInt16<TaskItemStatus>()))
                .ForMember(c => c.Priority, t => t.MapFrom(z => z.Priority.ToInt16<TaskItemStatus>()))
                .ForMember(c => c.Id, t => t.MapFrom(z => z.TaskId))
                .ForMember(c => c.MemberId, t => t.MapFrom(z => z.Member != null ? z.Member.AccountId : 0))
                .ForMember(c => c.PhaseId, t => t.MapFrom(z => z.Phase != null ? z.Phase.Id : 0))
                .ForMember(c => c.AssigneeId, t => t.MapFrom(z => z.Assignee != null ? z.Assignee.AccountId : 0))
                .ForMember(c => c.ProjectId, t => t.MapFrom(z => z.Project != null ? z.Project.ProjectId : 0));
        }

        public void NotificationMapper()
        {
            //Map Notification entity to Notification model
            CreateMap<Notification, NotificationModel>()
                .ForPath(c => c.Account.AccountId, t => t.MapFrom(z => z.Account.Id))
                .ForPath(c => c.Account.DisplayName, t => t.MapFrom(z => z.Account.AccountInfo.DisplayName))
                .ForMember(c => c.Context, t => t.MapFrom( z => z.Context.ToEnum<NotifyContext>()))
                .ReverseMap();

            //Map Notification model to Notification entity
            CreateMap<NotificationModel, Notification>()
                .ForMember(c => c.AccountId, t => t.MapFrom(z => z.Account.AccountId))
                .ForMember(c => c.Context, t => t.MapFrom(z => z.Context.ToInt16<NotifyContext>()));
        }

        public void TodoMapper()
        {
            //Map ToDoItem entity to ToDoItem model
            CreateMap<ToDoItem, ToDoItemModel>()
                .ForPath(c => c.Account.AccountId, t => t.MapFrom(z => z.Account.Id))
                .ForPath(c => c.Account.DisplayName, t => t.MapFrom(z => z.Account.AccountInfo.DisplayName))
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToEnum<ToDoItemStatus>()))
                .ForMember(c => c.Priority, t => t.MapFrom(z => z.Priority.ToEnum<ToDoItemPriority>()))
                .ReverseMap();

            //Map ToDoItem model to ToDoItem entity
            CreateMap<ToDoItemModel, ToDoItem>()
                .ForMember(c => c.Owner, t => t.MapFrom(z => z.Account.AccountId))
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToInt16<ToDoItemStatus>()))
                .ForMember(c => c.Priority, t => t.MapFrom(z => z.Priority.ToInt16<ToDoItemPriority>()));
        }
    }
}
