using EzTask.Entity;
using EzTask.Entity.Data;
using Microsoft.EntityFrameworkCore;
namespace EzTask.DataAccess
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
            AccountMap();
        }

        private void AccountMap()
        {
            _modelBuilder.Entity<Account>().ToTable(TableName.Account.ToString());
            _modelBuilder.Entity<AccountInfo>().ToTable(TableName.AccountInfo.ToString());
            _modelBuilder.Entity<Project>().ToTable(TableName.Project.ToString());
            _modelBuilder.Entity<ProjectMember>().ToTable(TableName.Project_Member.ToString());
            _modelBuilder.Entity<Skill>().ToTable(TableName.Skill.ToString());
            _modelBuilder.Entity<AccountSkill>().ToTable(TableName.Account_Skill.ToString());
            _modelBuilder.Entity<TaskItem>().ToTable(TableName.TaskItem.ToString());
        }
    }
}
