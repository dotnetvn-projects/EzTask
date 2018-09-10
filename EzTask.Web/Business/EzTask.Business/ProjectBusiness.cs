using EzTask.Entity.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class ProjectBusiness : BusinessCore
    {
        private readonly TaskBusiness _task;
        public ProjectBusiness(UnitOfWork unitOfWork,
            TaskBusiness task) : base(unitOfWork)
        {
            _task = task;
        }

        /// <summary>
        /// Save project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ProjectModel> Save(ProjectModel model)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = UnitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    Project project = model.ToEntity();
                    project.UpdatedDate = DateTime.Now;

                    if (project.Id < 1)
                    {
                        project.CreatedDate = DateTime.Now;
                        UnitOfWork.ProjectRepository.Add(project);
                    }
                    else
                    {
                        UnitOfWork.ProjectRepository.Update(project);
                    }

                    int iResult = await UnitOfWork.CommitAsync();

                    if (iResult > 0)
                    {
                        if (string.IsNullOrEmpty(project.ProjectCode))
                        {
                            project.ProjectCode = CreateCode("EzT", project.Id);

                            iResult = await UnitOfWork.CommitAsync();
                            if (iResult > 0)
                            {
                                //create feature in Phrase table as default
                                Phrase feature = new Phrase
                                {
                                    PhraseName = "Open Features",
                                    ProjectId = project.Id,
                                    Status = (int)PhraseStatus.Open
                                };
                                UnitOfWork.PhraseRepository.Add(feature);

                                //add project member
                                ProjectMember member = new ProjectMember
                                {
                                    MemberId = project.Owner,
                                    ProjectId = project.Id,
                                    AddDate = DateTime.Now
                                };
                                UnitOfWork.ProjectMemberRepository.Add(member);
                                await UnitOfWork.CommitAsync();
                            }
                        }
                    }
                    transaction.Commit();
                    return project.ToModel();
                }
                catch
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        /// <summary>
        /// Remove project
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> Delete(string projectCode)
        {
            ResultModel<bool> result = new ResultModel<bool>();

            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = UnitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    ProjectModel project = await GetProject(projectCode);
                    if (project != null)
                    {
                        //Remove member
                        IEnumerable<ProjectMember> memberList = await UnitOfWork.ProjectMemberRepository.GetManyAsync(c =>
                                c.ProjectId == project.ProjectId);
                        UnitOfWork.ProjectMemberRepository.DeleteRange(memberList);

                        //remove phrase
                        IEnumerable<Phrase> phrases = await UnitOfWork.PhraseRepository.GetManyAsync(c => c.ProjectId == project.ProjectId);
                        UnitOfWork.PhraseRepository.DeleteRange(phrases);

                        //remove task
                        await _task.DeleteTask(project.ProjectId);

                        //remove project
                        UnitOfWork.ProjectRepository.Delete(project.ToEntity());

                        await UnitOfWork.CommitAsync();
                    }

                    transaction.Commit();
                    result.Status = ActionStatus.Ok;
                    result.Data = true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    result.Status = ActionStatus.Failed;
                }

                return result;
            }
        }

        /// <summary>
        /// Get project list for a specific user
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectModel>> GetProjects(int ownerId)
        {
            List<Project> data = await UnitOfWork.ProjectMemberRepository.Entity.Include(c => c.Member)
                .Include(c => c.Project)
                .AsNoTracking()
                .Where(c => c.MemberId == ownerId)
                .OrderBy(c => c.Project.Status)
                .Select(x => x.Project)
                .ToListAsync();

            //TODO get project which related to project-member
            return data.ToModels();
        }

        /// <summary>
        ///  Get project
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProject(string projectCode)
        {
            Project data = await UnitOfWork.ProjectRepository.GetAsync(c =>
                c.ProjectCode == projectCode, allowTracking: false);

            return data.ToModel();
        }

        /// <summary>
        ///  Get project by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectByName(string name)
        {
            Project data = await UnitOfWork.ProjectRepository.GetAsync(c =>
                c.ProjectName.ToLower() == name.ToLower(), allowTracking: false);

            return data.ToModel();
        }

        /// <summary>
        /// Check the project is dupplicated or not
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>True if it is, False if it is not</returns>
        public async Task<bool> IsDupplicated(string name, int id)
        {
            Project data = await UnitOfWork.ProjectRepository.GetAsync(c =>
                 c.ProjectName.ToLower() == name.ToLower()
                 && c.Id != id, allowTracking: false);

            return data != null;
        }

        /// <summary>
        ///  Get project detail
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectDetail(string projectCode)
        {
            Project data = await UnitOfWork.ProjectRepository.Entity.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo).AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectCode == projectCode);

            return data.ToModel();
        }

        /// <summary>
        /// Get account list belong to a specific project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AccountModel>> GetAccountList(int projectId)
        {
            List<AccountModel> data = await UnitOfWork.ProjectMemberRepository.Entity
                .Include(c => c.Project)
                .Include(c => c.Member)
                .ThenInclude(c => c.AccountInfo).AsNoTracking()
                .Where(c => c.ProjectId == projectId).Select(t => new AccountModel
                {
                    AccountId = t.MemberId,
                    DisplayName = t.Member.AccountInfo.DisplayName
                }).ToListAsync();

            return data;
        }
    }
}
