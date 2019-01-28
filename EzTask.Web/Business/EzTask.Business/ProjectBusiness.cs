using EzTask.Entity.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Model;
using EzTask.Model.Enum;
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
                                //create feature in Phase table as default
                                Phase feature = new Phase
                                {
                                    PhaseName = "Open Features",
                                    ProjectId = project.Id,
                                    IsDefault = true,
                                    Status = (int)PhaseStatus.Open
                                };
                                UnitOfWork.PhaseRepository.Add(feature);

                                //add project member
                                ProjectMember member = new ProjectMember
                                {
                                    MemberId = project.Owner,
                                    ProjectId = project.Id,
                                    AddDate = DateTime.Now,
                                    IsPending = false
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
        /// Add member to project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<ProjectMemberModel>> AddMember(ProjectMemberModel model)
        {
            ResultModel<ProjectMemberModel> result = new ResultModel<ProjectMemberModel>();
            //add project member
            ProjectMember member = new ProjectMember
            {
                MemberId = model.AccountId,
                ProjectId = model.ProjectId,
                AddDate = DateTime.Now,
                IsPending = true
            };

            UnitOfWork.ProjectMemberRepository.Add(member);
            var iresult = await UnitOfWork.CommitAsync();

            if (iresult > 0)
            {
                var accountInfo = await UnitOfWork.AccountInfoRepository.GetAsync(c => 
                                                c.AccountId == model.AccountId);

                model.AddDate = member.AddDate;
                model.DisplayName = accountInfo.DisplayName;
                model.IsPending = member.IsPending;

                result.Data = model;
                result.Status = ActionStatus.Ok;
            }

            return result;
        }

        /// <summary>
        /// check member has been added to project yet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> HasAlreadyAdded(ProjectMemberModel model)
        {
            ResultModel<bool> result = new ResultModel<bool>();

            var data = await UnitOfWork.ProjectMemberRepository.GetAsync(c => c.MemberId == model.AccountId
                                 && c.ProjectId == model.ProjectId);

            if (data != null)
            {
                result.Data = true;
                result.Status = ActionStatus.Ok;
            }

            return result;
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

                        //remove phase
                        IEnumerable<Phase> phases = await UnitOfWork.PhaseRepository.GetManyAsync(c => c.ProjectId == project.ProjectId);
                        UnitOfWork.PhaseRepository.DeleteRange(phases);

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
            List<Project> data = await UnitOfWork.ProjectMemberRepository.Entity
                .Include(c => c.Member)
                .Include(c => c.Project).ThenInclude(c => c.Account).ThenInclude(c => c.AccountInfo)
                .AsNoTracking()
                .Where(c => c.MemberId == ownerId && c.IsPending == false)
                .OrderBy(c => c.Project.Status)
                .Select(x => new Project
                {
                    Account = new Account
                    {
                        AccountInfo = x.Project.Account.AccountInfo,
                        Id = x.Project.Account.Id
                    },
                    Comment = x.Project.Comment,
                    CreatedDate = x.Project.CreatedDate,
                    Description = x.Project.Description,
                    Id = x.Project.Id,
                    MaximumUser = x.Project.MaximumUser,
                    Owner = x.Project.Owner,
                    ProjectCode = x.Project.ProjectCode,
                    ProjectName = x.Project.ProjectName,
                    Status = x.Project.Status,
                    UpdatedDate = x.Project.UpdatedDate
                })
                .ToListAsync();

            return data.ToModels();
        }

        /// <summary>
        /// Count project by user
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<int> CountByUser(int ownerId)
        {
            var data = await UnitOfWork.ProjectMemberRepository
                            .Entity
                            .CountAsync(c => c.MemberId == ownerId && c.IsPending == false);

            return data;
        }


        /// <summary>
        ///  Get project
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProject(string projectCode)
        {
            Project data = await UnitOfWork.ProjectRepository
                .Entity
                .Include(c => c.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectCode == projectCode);

            return data.ToModel();
        }

        /// <summary>
        ///  Get project
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectByOwner(int owner)
        {
            Project data = await UnitOfWork.ProjectRepository
                .Entity
                .Include(c => c.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Owner == owner);

            return data.ToModel();
        }

        /// <summary>
        ///  Get project
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProject(int projectId)
        {
            Project data = await UnitOfWork.ProjectRepository
                .Entity
                .Include(c => c.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == projectId);

            return data.ToModel();
        }

        /// <summary>
        ///  Get project by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectByName(string name)
        {
            Project data = await UnitOfWork.ProjectRepository
                .GetAsync(c =>c.ProjectName.ToLower() == name.ToLower(), allowTracking: false);

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
            Project data = await UnitOfWork.ProjectRepository
                .GetAsync(c => c.ProjectName.ToLower() == name.ToLower()
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
            Project data = await UnitOfWork.ProjectRepository
                .Entity
                .Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo).AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectCode == projectCode);

            var model = data.ToModel();

            return model;
        }

        /// <summary>
        /// Count member of project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<int> CountMember(int projectId)
        {
            var data = await UnitOfWork.ProjectMemberRepository
                .Entity
                .CountAsync(c => c.ProjectId == projectId);

            return data;
        }

        /// <summary>
        /// Get account list belong to a specific project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectMemberModel>> GetAccountList(int projectId)
        {
            List<ProjectMemberModel> data = await UnitOfWork.ProjectMemberRepository
                .Entity
                .Include(c => c.Project)
                .Include(c => c.Member)
                .ThenInclude(c => c.AccountInfo)
                .AsNoTracking()
                .Where(c => c.ProjectId == projectId)
                .Select(t => new ProjectMemberModel
                {
                    ProjectId = t.ProjectId,
                    AccountId = t.MemberId,
                    IsPending = t.IsPending,
                    AddDate = t.AddDate,
                    DisplayName = t.Member.AccountInfo.DisplayName,
                }).ToListAsync();

            return data;
        }

        /// <summary>
        /// Get account id list belong to a specific project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetAccountIdList(int projectId)
        {
            List<int> data = await UnitOfWork.ProjectMemberRepository
                .Entity.AsNoTracking()
                .Where(c => c.ProjectId == projectId)
                .Select(t => t.MemberId)
                .ToListAsync();

            return data;
        }
    }
}
