using EzTask.Entity.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Framework.IO;
using EzTask.Framework.Security;
using EzTask.Interface;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Plugin.MessageService.Data.Email;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class ProjectBusiness : BusinessCore
    {
        private readonly TaskBusiness _task;
        private readonly AccountBusiness _accountBusiness;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMessageCenter _mesageCenter;
        private readonly IAccountContext _accountContext;

        public ProjectBusiness(UnitOfWork unitOfWork,
            TaskBusiness task, IWebHostEnvironment hostEnvironment,
            IMessageCenter mesageCenter, AccountBusiness accountBusiness,
            IAccountContext accountContext) : base(unitOfWork)
        {
            _task = task;
            _hostEnvironment = hostEnvironment;
            _mesageCenter = mesageCenter;
            _accountBusiness = accountBusiness;
            _accountContext = accountContext;
        }

        /// <summary>
        /// Save project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<ProjectModel>> Save(ProjectModel model)
        {
            ResultModel<ProjectModel> result = new ResultModel<ProjectModel>();

            if (model.Owner.AccountId != _accountContext.AccountId)
            {
                result.Status = ActionStatus.UnAuthorized;
            }
            else
            {
                using (IDbContextTransaction transaction = UnitOfWork.Context.Database.BeginTransaction())
                {
                    try
                    {
                        Project project = model.ToEntity();
                        project.UpdatedDate = DateTime.Now;
                        project.MaximumUser = 9999;

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
                        result.Status = ActionStatus.Ok;
                        result.Data = project.ToModel();
                    }
                    catch
                    {
                        result.Status = ActionStatus.Failed;
                        transaction.Rollback();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Add member to project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<ProjectMemberModel>> AddMember(ProjectMemberModel model)
        {
            ResultModel<ProjectMemberModel> result = new ResultModel<ProjectMemberModel>();
            if (!IsOwner(model.ProjectId))
            {
                result.Status = ActionStatus.UnAuthorized;
            }
            else
            {
                //add project member
                ProjectMember member = new ProjectMember
                {
                    MemberId = model.AccountId,
                    ProjectId = model.ProjectId,
                    AddDate = DateTime.Now,
                    IsPending = true
                };

                UnitOfWork.ProjectMemberRepository.Add(member);
                int iresult = await UnitOfWork.CommitAsync();

                if (iresult > 0)
                {
                    AccountInfo accountInfo = await UnitOfWork.AccountInfoRepository.GetAsync(c =>
                                                    c.AccountId == model.AccountId);

                    model.AddDate = member.AddDate;
                    model.DisplayName = accountInfo.DisplayName;
                    model.IsPending = member.IsPending;

                    result.Data = model;
                    result.Status = ActionStatus.Ok;
                }
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

            ProjectMember data = await UnitOfWork.ProjectMemberRepository.GetAsync(c => c.MemberId == model.AccountId
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

            using (IDbContextTransaction transaction = UnitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    ProjectModel project = await GetProject(projectCode);

                    if (project.Owner.AccountId != _accountContext.AccountId)
                    {
                        result.Status = ActionStatus.UnAuthorized;
                    }
                    else
                    {
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
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectModel>> GetProjects(int accountId)
        {
            List<Project> data = await UnitOfWork.ProjectMemberRepository.Entity
                .Include(c => c.Member)
                .Include(c => c.Project).ThenInclude(c => c.Account).ThenInclude(c => c.AccountInfo)
                .AsNoTracking()
                .Where(c => c.MemberId == accountId && c.IsPending == false)
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
        /// Get project id list for a specific user
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetProjectIds(int accountId)
        {
            List<int> data = await UnitOfWork.ProjectMemberRepository.Entity
                .Include(c => c.Member)
                .Include(c => c.Project)
                .AsNoTracking()
                .Where(c => c.MemberId == accountId && c.IsPending == false)
                .Select(x => x.ProjectId)
                .ToListAsync();

            return data;
        }

        /// <summary>
        /// Count project by user
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<int> CountProjectByUser(int accountId)
        {
            int data = await UnitOfWork.ProjectMemberRepository
                            .Entity
                            .CountAsync(c => c.MemberId == accountId && c.IsPending == false);

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
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectByOwner(int accountId)
        {
            Project data = await UnitOfWork.ProjectRepository
                .Entity
                .Include(c => c.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Owner == accountId);

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
                .GetAsync(c => c.ProjectName.ToLower() == name.ToLower(), allowTracking: false);

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

            ProjectModel model = data.ToModel();

            return model;
        }

        /// <summary>
        /// Count member of project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<int> CountMemberByProjectId(int projectId)
        {
            int data = await UnitOfWork.ProjectMemberRepository
                .Entity
                .CountAsync(c => c.ProjectId == projectId);

            return data;
        }

        /// <summary>
        /// Count member of projects
        /// </summary>
        /// <param name="projectIds"></param>
        /// <returns></returns>
        public async Task<int> CountMemberByProductIdList(List<int> projectIds)
        {
            int data = await UnitOfWork.ProjectMemberRepository
                .Entity
                .CountAsync(c => projectIds.Contains(c.ProjectId));

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

        /// <summary>
        /// Accept invitation
        /// </summary>
        /// <param name="activeCode"></param>
        /// <returns></returns>
        public async Task<ResultModel<AccountInfoModel>> AcceptInvitation(string activeCode)
        {
            ResultModel<AccountInfoModel> iResult = new ResultModel<AccountInfoModel>();
            ProjectMember inviteItem = await UnitOfWork.ProjectMemberRepository.GetAsync(c => c.ActiveCode == activeCode);

            if (inviteItem == null)
            {
                iResult.Status = ActionStatus.NotFound;
            }
            else
            {
                inviteItem.ActiveCode = string.Empty;
                inviteItem.IsPending = false;

                UnitOfWork.ProjectMemberRepository.Update(inviteItem);

                if (await UnitOfWork.CommitAsync() > 0)
                {
                    iResult.Data = await _accountBusiness.GetAccountInfo(inviteItem.MemberId);
                    iResult.Status = ActionStatus.Ok;
                }
            }
            return iResult;
        }

        /// <summary>
        /// Get project info by active code
        /// </summary>
        /// <param name="activeCode"></param>
        /// <returns></returns>
        public async Task<ResultModel<ProjectModel>> GetProjectByActiveCode(string activeCode)
        {
            ResultModel<ProjectModel> iResult = new ResultModel<ProjectModel>();
            var project = await UnitOfWork.ProjectMemberRepository.Entity
                .Include(c => c.Project)
                .ThenInclude(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .Select(t => new ProjectModel
                {
                    Owner = new AccountModel
                    {
                        DisplayName = t.Project.Account.AccountInfo.DisplayName
                    },
                    ProjectName = t.Project.ProjectName

                }).FirstOrDefaultAsync();

            if (project != null)
            {
                iResult.Status = ActionStatus.Ok;
                iResult.Data = project;
            }

            return iResult;
        }

        /// <summary>
        /// Send an email to invite an user joins project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task SendInvitation(int projectId, int memberId, bool isNewMember,
            string title, string activeCode)
        {
            string emailTemplateUrl = _hostEnvironment.GetRootContentUrl()
                       + "/resources/templates/invite_email.html";
            AccountInfoModel member = await _accountBusiness.GetAccountInfo(memberId);
            ProjectModel project = await GetProject(projectId);
            string password = string.Empty;

            if (isNewMember)
            {
                emailTemplateUrl = _hostEnvironment.GetRootContentUrl()
                       + "/resources/templates/invite_new_email.html";

                string hash = Cryptography.GetHashString(member.AccountName);
                password = Decrypt.Do(member.Password, hash);
            }

            string emailContent = StreamIO.ReadFile(emailTemplateUrl);
            emailContent = emailContent.Replace("{UserName}", member.DisplayName);
            emailContent = emailContent.Replace("{Project}", project.ProjectName.ToUpper());
            emailContent = emailContent.Replace("{Url}", "http://eztask.dotnetvn.com/project/accept-invite.html?ref=" + activeCode);
            emailContent = emailContent.Replace("{Account}", member.AccountName);
            emailContent = emailContent.Replace("{Password}", password);

            _mesageCenter.Push(new EmailMessage
            {
                Content = emailContent,
                Title = title + " " + project.ProjectName.ToUpper(),
                To = member.AccountName
            });
        }

        private bool IsOwner(int projectId)
        {
            var project = UnitOfWork.ProjectRepository.GetById(projectId, allowTracking: false);

            return project.Owner == _accountContext.AccountId;
        }
    }
}
