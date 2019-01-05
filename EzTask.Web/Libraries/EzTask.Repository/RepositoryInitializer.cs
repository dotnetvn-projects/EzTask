using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Repository
{
    public static class RepositoryInitializer
    {
        public static void Register(IServiceCollection services)
        {            
            services.AddTransient<IRepository<Account>, TRepository<Account>>();
            services.AddTransient<IRepository<AccountInfo>, TRepository<AccountInfo>>();
            services.AddTransient<IRepository<Project>, TRepository<Project>>();
            services.AddTransient<IRepository<Phrase>, TRepository<Phrase>>();
            services.AddTransient<IRepository<AccountSkill>, TRepository<AccountSkill>>();
            services.AddTransient<IRepository<ProjectMember>, TRepository<ProjectMember>>();
            services.AddTransient<IRepository<Skill>, TRepository<Skill>>();
            services.AddTransient<IRepository<TaskItem>, TRepository<TaskItem>>();
            services.AddTransient<IRepository<Attachment>, TRepository<Attachment>>();
            services.AddTransient<IRepository<TaskHistory>, TRepository<TaskHistory>>();
            services.AddTransient<IRepository<Notification>, TRepository<Notification>>();
            services.AddTransient<UnitOfWork>();
        }
    }
}
