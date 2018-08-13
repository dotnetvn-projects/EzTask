using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Interfaces;
using System;
using System.Threading.Tasks;

namespace EzTask.Repository
{
    public class UnitOfWork : IDisposable
    {
        public EzTaskDbContext Context { get; }

        public IRepository<Account> AccountRepository { get; }
        public IRepository<AccountInfo> AccountInfoRepository { get; }
        public IRepository<Project> ProjectRepository { get; }
        public IRepository<Phrase> PhraseRepository { get; }
        public IRepository<AccountSkill> AccountSkillRepository { get; }
        public IRepository<ProjectMember> ProjectMemberRepository { get; }
        public IRepository<Skill> SkillRepository { get; }
        public IRepository<TaskItem> TaskRepository { get; }
        public IRepository<Attachment> AttachRepository { get; }

        public UnitOfWork(EzTaskDbContext context,
             IRepository<Account> account,
             IRepository<AccountInfo> accountInfo,
             IRepository<Project> project,
             IRepository<Phrase> phrase,
             IRepository<AccountSkill> accountSkill,
             IRepository<ProjectMember> projectMember,
             IRepository<Skill> skill,
             IRepository<TaskItem> task,
             IRepository<Attachment> attach)
        {
            Context = context;
            AccountRepository = account;
            AccountRepository.Context = Context;

            AccountInfoRepository = accountInfo;
            AccountInfoRepository.Context = Context;

            ProjectRepository = project;
            ProjectRepository.Context = Context;

            PhraseRepository = phrase;
            PhraseRepository.Context = context;

            AccountSkillRepository = accountSkill;
            AccountSkillRepository.Context = context;

            ProjectMemberRepository = projectMember;
            ProjectMemberRepository.Context = context;

            SkillRepository = skill;
            SkillRepository.Context = context;

            TaskRepository = task;
            TaskRepository.Context = context;

            AttachRepository = attach;
            AttachRepository.Context = context;
        }



        public int Commit()
        {
            var iResult = Context.SaveChanges();
            return iResult;
        }

        public async Task<int> CommitAsync()
        {
            var iResult = await Context.SaveChangesAsync();
            return iResult;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
