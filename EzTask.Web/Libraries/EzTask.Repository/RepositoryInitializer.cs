using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Repository
{
    public static class RepositoryInitializer
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork<EzTaskDbContext>, UnitOfWork>();
            services.AddTransient<IRepository<Account>, TRepository<Account>>();
            services.AddTransient<IRepository<AccountInfo>, TRepository<AccountInfo>>();
            services.AddTransient<IRepository<Project>, TRepository<Project>>();
            services.AddTransient<IRepository<Phrase>, TRepository<Phrase>>();
            services.AddTransient<IRepository<AccountSkill>, TRepository<AccountSkill>>();
            services.AddTransient<IRepository<ProjectMember>, TRepository<ProjectMember>>();
            services.AddTransient<IRepository<Skill>, TRepository<Skill>>();
            services.AddTransient<IRepository<TaskItem>, TRepository<TaskItem>>();
        }
    }
}
