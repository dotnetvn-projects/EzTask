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

        public static IEnumerable<AccountModel> ToModels(this IEnumerable<Account> entity)
        {
            return ModelRegister.Mapper.Map<IEnumerable<AccountModel>>(entity);
        }

        public static AccountInfoModel ToModel(this AccountInfo entity)
        {
            return ModelRegister.Mapper.Map<AccountInfoModel>(entity);
        }

        public static AccountInfo ToEntity(this AccountInfoModel model)
        {
            return ModelRegister.Mapper.Map<AccountInfo>(model);
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

        public static IEnumerable<ProjectModel> ToModels(this IEnumerable<Project> entity)
        {
            var data = ModelRegister.Mapper.Map<IEnumerable<ProjectModel>>(entity);
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

        public static IEnumerable<PhaseModel> ToModels(this IEnumerable<Phase> entity)
        {
            return ModelRegister.Mapper.Map<IEnumerable<PhaseModel>>(entity);
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

        public static IEnumerable<TaskItemModel> ToModels(this IEnumerable<TaskItem> entity)
        {
            return ModelRegister.Mapper.Map<IEnumerable<TaskItemModel>>(entity);
        }

        public static Attachment ToEntity(this AttachmentModel model)
        {
            return ModelRegister.Mapper.Map<Attachment>(model);
        }

        public static AttachmentModel ToModel(this Attachment entity)
        {
            return ModelRegister.Mapper.Map<AttachmentModel>(entity);
        }

        public static IEnumerable<AttachmentModel> ToModels(this IEnumerable<Attachment> entity)
        {
            return ModelRegister.Mapper.Map<IEnumerable<AttachmentModel>>(entity);
        }

        public static TaskHistory ToEntity(this TaskHistoryModel model)
        {
            return ModelRegister.Mapper.Map<TaskHistory>(model);
        }

        public static TaskHistoryModel ToModel(this TaskHistory entity)
        {
            return ModelRegister.Mapper.Map<TaskHistoryModel>(entity);
        }

        public static IEnumerable<TaskHistoryModel> ToModels(this IEnumerable<TaskHistory> entity)
        {
            return ModelRegister.Mapper.Map<IEnumerable<TaskHistoryModel>>(entity);
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

        public static IEnumerable<NotificationModel> ToModels(this IEnumerable<Notification> entity)
        {
            return ModelRegister.Mapper.Map<IEnumerable<NotificationModel>>(entity);
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

        public static IEnumerable<ToDoItemModel> ToModels(this IEnumerable<ToDoItem> entity)
        {
            return ModelRegister.Mapper.Map<IEnumerable<ToDoItemModel>>(entity);
        }
        #endregion
    }

}
