using EzTask.Entity.Data;
using EzTask.Model;
using EzTask.Model.Enum;
using System.Collections.Generic;

namespace EzTask.Model
{
    public static class DataMapper
    {
        #region Account Mapper

        public static Account ToEntity(this AccountModel model)
        {
            return ModelRegister.Mapper.Map<Account>(model);
        }

        public static AccountModel ToModel(this Account entity)
        {
            return ModelRegister.Mapper.Map<AccountModel>(entity);
        }

        public static IList<AccountModel> ToModels(this IList<Account> entity)
        {
            return ModelRegister.Mapper.Map<IList<AccountModel>>(entity);
        }

        public static AccountInfoModel ToModel(this AccountInfo entity)
        {
            return ModelRegister.Mapper.Map<AccountInfoModel>(entity);
        }

        public static AccountInfo ToEntity(this AccountInfoModel model)
        {
            return ModelRegister.Mapper.Map<AccountInfo>(model);
        }

        public static RecoverSession ToEntity(this RecoverSessionModel model)
        {
            return ModelRegister.Mapper.Map<RecoverSession>(model);
        }

        public static RecoverSessionModel ToModel(this RecoverSession entity)
        {
            return ModelRegister.Mapper.Map<RecoverSessionModel>(entity);
        }

        #endregion

        #region Project

        public static ProjectModel ToModel(this Project entity)
        {
            return ModelRegister.Mapper.Map<ProjectModel>(entity);
        }

        public static Project ToEntity(this ProjectModel model)
        {
            return ModelRegister.Mapper.Map<Project>(model);
        }

        public static IList<ProjectModel> ToModels(this IList<Project> entity)
        {
            var data = ModelRegister.Mapper.Map<IList<ProjectModel>>(entity);
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

        #region Phase Mapper
        public static PhaseModel ToModel(this Phase entity)
        {
            return ModelRegister.Mapper.Map<PhaseModel>(entity);
        }

        public static Phase ToEntity(this PhaseModel model)
        {
            return ModelRegister.Mapper.Map<Phase>(model);
        }

        public static IList<PhaseModel> ToModels(this IList<Phase> entity)
        {
            return ModelRegister.Mapper.Map<IList<PhaseModel>>(entity);
        }
        #endregion

        #region Task Mapper
        public static TaskItemModel ToModel(this TaskItem entity)
        {
            return ModelRegister.Mapper.Map<TaskItemModel>(entity);
        }

        public static TaskItem ToEntity(this TaskItemModel model)
        {
            return ModelRegister.Mapper.Map<TaskItem>(model);
        }

        public static IList<TaskItemModel> ToModels(this IList<TaskItem> entity)
        {
            return ModelRegister.Mapper.Map<IList<TaskItemModel>>(entity);
        }

        public static Attachment ToEntity(this AttachmentModel model)
        {
            return ModelRegister.Mapper.Map<Attachment>(model);
        }

        public static AttachmentModel ToModel(this Attachment entity)
        {
            return ModelRegister.Mapper.Map<AttachmentModel>(entity);
        }

        public static IList<AttachmentModel> ToModels(this IList<Attachment> entity)
        {
            return ModelRegister.Mapper.Map<IList<AttachmentModel>>(entity);
        }

        public static TaskHistory ToEntity(this TaskHistoryModel model)
        {
            return ModelRegister.Mapper.Map<TaskHistory>(model);
        }

        public static TaskHistoryModel ToModel(this TaskHistory entity)
        {
            return ModelRegister.Mapper.Map<TaskHistoryModel>(entity);
        }

        public static IList<TaskHistoryModel> ToModels(this IList<TaskHistory> entity)
        {
            return ModelRegister.Mapper.Map<IList<TaskHistoryModel>>(entity);
        }
        #endregion

        #region Notification Mapper
        public static Notification ToEntity(this NotificationModel model)
        {
            return ModelRegister.Mapper.Map<Notification>(model);
        }

        public static NotificationModel ToModel(this Notification entity)
        {
            return ModelRegister.Mapper.Map<NotificationModel>(entity);
        }

        public static IList<NotificationModel> ToModels(this IList<Notification> entity)
        {
            return ModelRegister.Mapper.Map<IList<NotificationModel>>(entity);
        }
        #endregion

        #region To Do List Mapper
        public static ToDoItem ToEntity(this ToDoItemModel model)
        {
            return ModelRegister.Mapper.Map<ToDoItem>(model);
        }

        public static ToDoItemModel ToModel(this ToDoItem entity)
        {
            return ModelRegister.Mapper.Map<ToDoItemModel>(entity);
        }

        public static IList<ToDoItemModel> ToModels(this IList<ToDoItem> entity)
        {
            return ModelRegister.Mapper.Map<IList<ToDoItemModel>>(entity);
        }
        #endregion
    }

}
