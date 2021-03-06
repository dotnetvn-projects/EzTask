﻿using EzTask.Database;
using EzTask.Entity.Data;
using EzTask.Interface;
using System;
using System.Threading.Tasks;

namespace EzTask.Repository
{
    public class UnitOfWork : IDisposable
    {
        public DataContext Context { get; }

        public IRepository<Account> AccountRepository { get; }
        public IRepository<AccountInfo> AccountInfoRepository { get; }
        public IRepository<Project> ProjectRepository { get; }
        public IRepository<Phase> PhaseRepository { get; }
        public IRepository<AccountSkill> AccountSkillRepository { get; }
        public IRepository<ProjectMember> ProjectMemberRepository { get; }
        public IRepository<Skill> SkillRepository { get; }
        public IRepository<TaskItem> TaskRepository { get; }
        public IRepository<Attachment> AttachRepository { get; }
        public IRepository<TaskHistory> TaskHistoryRepository { get; }
        public IRepository<Notification> NotifyRepository { get; }
        public IRepository<ToDoItem> TodoItemRepository { get; }
        public IRepository<RecoverSession> RecoverSessionRepository { get; }

        public UnitOfWork(DataContext context,
             IRepository<Account> account,
             IRepository<AccountInfo> accountInfo,
             IRepository<Project> project,
             IRepository<Phase> phase,
             IRepository<AccountSkill> accountSkill,
             IRepository<ProjectMember> projectMember,
             IRepository<Skill> skill,
             IRepository<TaskItem> task,
             IRepository<Attachment> attach,
             IRepository<TaskHistory> taskHistory,
             IRepository<Notification> notifyRepository,
             IRepository<ToDoItem> todoItemRepository,
             IRepository<RecoverSession> recoverSessionRepository)
        {
            Context = context;

            AccountRepository = account;
            AccountRepository.Context = Context;

            AccountInfoRepository = accountInfo;
            AccountInfoRepository.Context = Context;

            ProjectRepository = project;
            ProjectRepository.Context = Context;

            PhaseRepository = phase;
            PhaseRepository.Context = context;

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

            TaskHistoryRepository = taskHistory;
            TaskHistoryRepository.Context = context;

            NotifyRepository = notifyRepository;
            NotifyRepository.Context = context;

            TodoItemRepository = todoItemRepository;
            TodoItemRepository.Context = context;

            RecoverSessionRepository = recoverSessionRepository;
            RecoverSessionRepository.Context = context;
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
