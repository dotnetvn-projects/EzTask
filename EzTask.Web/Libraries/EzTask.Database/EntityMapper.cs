﻿using EzTask.Entity.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EzTask.Database
{
    internal class EntityMapper
    {
        private readonly ModelBuilder _modelBuilder ;
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

            foreach (var relationship in _modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}