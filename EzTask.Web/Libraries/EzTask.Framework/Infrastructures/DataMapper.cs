using EzTask.Entity.Data;
using EzTask.Models;
using EzTask.Models.Enum;
using System.Collections.Generic;

namespace EzTask.Framework.Infrastructures
{
    public static class DataMapper
    {
        #region Account Mapper

        public static Account ToEntity(this RegisterModel model)
        {
            return FrameworkCore.Mapper.Map<Account>(model);
        }

        public static AccountModel ToModel(this Account entity)
        {
            return FrameworkCore.Mapper.Map<AccountModel>(entity);
        }

        public static IEnumerable<AccountModel> ToModels(this IEnumerable<Account> entity)
        {
            return FrameworkCore.Mapper.Map<IEnumerable<AccountModel>>(entity);
        }

        public static AccountInfoModel ToModel(this AccountInfo entity)
        {
            if (entity == null)
                return new AccountInfoModel();

            return FrameworkCore.Mapper.Map<AccountInfoModel>(entity);
        }

        public static AccountInfo ToEntity(this AccountInfoModel model)
        {
            return FrameworkCore.Mapper.Map<AccountInfo>(model);
        }
        #endregion

        #region Project

        public static ProjectModel ToModel(this Project entity)
        {
            if (entity == null)
                return null;

            return FrameworkCore.Mapper.Map<ProjectModel>(entity);
        }

        public static Project ToEntity(this ProjectModel model)
        {
            if (model == null)
                return null;

            return FrameworkCore.Mapper.Map<Project>(model);
        }

        public static IEnumerable<ProjectModel> ToModels(this IEnumerable<Project> entity)
        {
            var data = FrameworkCore.Mapper.Map<IEnumerable<ProjectModel>>(entity);
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
            if (entity == null)
                return null;

            return FrameworkCore.Mapper.Map<PhaseModel>(entity);
        }

        public static Phase ToEntity(this PhaseModel model)
        {
            if (model == null)
                return null;

            return FrameworkCore.Mapper.Map<Phase>(model);
        }

        public static IEnumerable<PhaseModel> ToModels(this IEnumerable<Phase> entity)
        {
            return FrameworkCore.Mapper.Map<IEnumerable<PhaseModel>>(entity);
        }
        #endregion

        #region Task Mapper
        public static TaskItemModel ToModel(this TaskItem entity)
        {
            if (entity == null)
                return null;

            return FrameworkCore.Mapper.Map<TaskItemModel>(entity);
        }

        public static TaskItem ToEntity(this TaskItemModel model)
        {
            if (model == null)
                return null;

            return FrameworkCore.Mapper.Map<TaskItem>(model);
        }

        public static IEnumerable<TaskItemModel> ToModels(this IEnumerable<TaskItem> entity)
        {
            return FrameworkCore.Mapper.Map<IEnumerable<TaskItemModel>>(entity);
        }

        public static Attachment ToEntity(this AttachmentModel model)
        {
            return FrameworkCore.Mapper.Map<Attachment>(model);
        }

        public static AttachmentModel ToModel(this Attachment entity)
        {
            return FrameworkCore.Mapper.Map<AttachmentModel>(entity);
        }

        public static IEnumerable<AttachmentModel> ToModels(this IEnumerable<Attachment> entity)
        {
            return FrameworkCore.Mapper.Map<IEnumerable<AttachmentModel>>(entity);
        }

        public static TaskHistory ToEntity(this TaskHistoryModel model)
        {
            return FrameworkCore.Mapper.Map<TaskHistory>(model);
        }

        public static TaskHistoryModel ToModel(this TaskHistory entity)
        {
            return FrameworkCore.Mapper.Map<TaskHistoryModel>(entity);
        }

        public static IEnumerable<TaskHistoryModel> ToModels(this IEnumerable<TaskHistory> entity)
        {
            return FrameworkCore.Mapper.Map<IEnumerable<TaskHistoryModel>>(entity);
        }
        #endregion

        #region Notification Mapper
        public static Notification ToEntity(this NotificationModel model)
        {
            return FrameworkCore.Mapper.Map<Notification>(model);
        }

        public static NotificationModel ToModel(this Notification entity)
        {
            return FrameworkCore.Mapper.Map<NotificationModel>(entity);
        }

        public static IEnumerable<NotificationModel> ToModels(this IEnumerable<Notification> entity)
        {
            return FrameworkCore.Mapper.Map<IEnumerable<NotificationModel>>(entity);
        }
        #endregion
    }

}
