using EzTask.Entity.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EzTask.Database
{
    internal class EntityMapper
    {
        private readonly ModelBuilder _modelBuilder;
        public EntityMapper(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Map()
        {
            TableMap();
        }

        private void TableMap()
        {
            _modelBuilder.Entity<Account>().ToTable(TableName.Account.ToString());
            _modelBuilder.Entity<AccountInfo>().ToTable(TableName.AccountInfo.ToString());
            _modelBuilder.Entity<Project>().ToTable(TableName.Project.ToString());
            _modelBuilder.Entity<ProjectMember>().ToTable(TableName.Project_Member.ToString());
            _modelBuilder.Entity<Skill>().ToTable(TableName.Skill.ToString());
            _modelBuilder.Entity<AccountSkill>().ToTable(TableName.Account_Skill.ToString());
            _modelBuilder.Entity<TaskItem>().ToTable(TableName.TaskItem.ToString());
            _modelBuilder.Entity<Phase>().ToTable(TableName.Phase.ToString());
            _modelBuilder.Entity<Attachment>().ToTable(TableName.Attachment.ToString());
            _modelBuilder.Entity<TaskHistory>().ToTable(TableName.TaskHistory.ToString());
            _modelBuilder.Entity<Notification>().ToTable(TableName.Notification.ToString());
            _modelBuilder.Entity<ToDoItem>().ToTable(TableName.ToDoItem.ToString());
            _modelBuilder.Entity<RecoverSession>().ToTable(TableName.RecoverSession.ToString());

            _modelBuilder.Entity<Account>().HasIndex(x => new 
            { 
                x.AccountName,
                x.PasswordHash,
                x.ManageAccountId,
                x.Password
            });

            _modelBuilder.Entity<AccountInfo>().HasIndex(x => new
            { 
                x.AccountId,
                x.IsPublished,
                x.Email
            });

            _modelBuilder.Entity<AccountSkill>().HasIndex(x => new
            {
                x.AccountId,
                x.SkillId
            });

            _modelBuilder.Entity<Attachment>().HasIndex(x => new
            {
                x.AddedUser,
                x.TaskId
            });

            _modelBuilder.Entity<Notification>().HasIndex(x => new
            {
                x.AccountId,
                x.CreatedDate
            });

            _modelBuilder.Entity<Phase>().HasIndex(x => new
            {
                x.IsDefault,
                x.ProjectId
            });

            _modelBuilder.Entity<Project>().HasIndex(x => new
            {
                x.ProjectCode,
                x.Owner,
                x.Status
            });

            _modelBuilder.Entity<ProjectMember>().HasIndex(x => new
            {
                x.ActiveCode,
                x.IsPending,
                x.MemberId,
                x.ProjectId
            });

            _modelBuilder.Entity<RecoverSession>().HasIndex(x => new
            {
                x.AccountId,
                x.Code,
                x.IsUsed
            });

            _modelBuilder.Entity<TaskHistory>().HasIndex(x => new
            {
                x.TaskId,
                x.UpdatedUser,
                x.UpdatedDate
            });

            _modelBuilder.Entity<TaskItem>().HasIndex(x => new
            {
                x.AssigneeId,
                x.MemberId,
                x.PhaseId,
                x.ProjectId
            });

            _modelBuilder.Entity<ToDoItem>().HasIndex(x => new
            {
                x.Owner,
                x.Status,
                x.UpdatedDate,
                x.ManagedCode
            });


            foreach (var relationship in _modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
