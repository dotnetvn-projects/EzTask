using EzTask.Entity.Data;
using EzTask.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Repository
{
    public static class RepositoryRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IRepository<Account>, TRepository<Account>>();
            services.AddScoped<IRepository<AccountInfo>, TRepository<AccountInfo>>();
            services.AddScoped<IRepository<Project>, TRepository<Project>>();
            services.AddScoped<IRepository<Phase>, TRepository<Phase>>();
            services.AddScoped<IRepository<AccountSkill>, TRepository<AccountSkill>>();
            services.AddScoped<IRepository<ProjectMember>, TRepository<ProjectMember>>();
            services.AddScoped<IRepository<Skill>, TRepository<Skill>>();
            services.AddScoped<IRepository<TaskItem>, TRepository<TaskItem>>();
            services.AddScoped<IRepository<Attachment>, TRepository<Attachment>>();
            services.AddScoped<IRepository<TaskHistory>, TRepository<TaskHistory>>();
            services.AddScoped<IRepository<Notification>, TRepository<Notification>>();
            services.AddScoped<IRepository<ToDoItem>, TRepository<ToDoItem>>();
            services.AddScoped<IRepository<RecoverSession>, TRepository<RecoverSession>>();
            services.AddScoped<UnitOfWork>();
        }
    }
}
