using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Interfaces;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class ProjectBusiness : BaseBusiness<EzTaskDbContext>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<Phrase> _phraseRepository;
        private readonly IRepository<ProjectMember> _projectMemberRepository;

        public ProjectBusiness(
            IRepository<Project> projectRepository,
            IRepository<Phrase> phraseRepository,
            IRepository<ProjectMember> projectMemberRepository) 
        {
            _projectRepository = projectRepository;
            _phraseRepository = phraseRepository;
            _projectMemberRepository = projectMemberRepository;
        }

        /// <summary>
        /// Save project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ProjectModel> Save(ProjectModel model)
        {
            using (var transaction = UnitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    var project = model.ToEntity();
                    project.UpdatedDate = DateTime.Now;

                    if (project.Id < 1)
                    {
                        project.CreatedDate = DateTime.Now;
                        _projectRepository.Add(project);
                    }
                    else
                    {
                        _projectRepository.Update(project);
                    }

                    var iResult = await UnitOfWork.CommitAsync();

                    if (iResult > 0)
                    {
                        if (string.IsNullOrEmpty(project.ProjectCode))
                        {
                            project.ProjectCode = CreateCode("EzT", project.Id);
                          
                            iResult = await UnitOfWork.CommitAsync();
                            if (iResult > 0)
                            {
                                //create feature in Phrase table as default
                                var feature = new Phrase
                                {
                                    PhraseName = "Open Features",
                                    ProjectId = project.Id,
                                    Status = (int)PhraseStatus.Open
                                };

                                _phraseRepository.Add(feature);
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

            using (var transaction = UnitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    var project = await GetProject(projectCode);
                    if (project != null)
                    {
                        //Remove member
                        var memberList = await _projectMemberRepository.GetManyAsync(c => c.ProjectId == project.ProjectId);
                        _projectMemberRepository.DeleteRange(memberList);

                        //remove phrase
                        var phrases = await _phraseRepository.GetManyAsync(c => c.ProjectId == project.ProjectId);
                        _phraseRepository.DeleteRange(phrases);

                        //remove project
                        _projectRepository.Delete(project.ToEntity());

                        await UnitOfWork.CommitAsync();
                    }

                    transaction.Commit();
                    result.Status = ActionStatus.Ok;
                    result.Data = true;
                }
                catch (Exception )
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
            var data =  await _projectRepository.Entity.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .AsNoTracking()
                .Where(c => c.Owner == ownerId).OrderBy(c => c.Status)
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
            var data = await _projectRepository.GetAsync(c => c.ProjectCode == projectCode, allowTracking: false);
            return data.ToModel();
        }

        /// <summary>
        ///  Get project by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectByName(string name)
        {
            var data = await _projectRepository.GetAsync(c => c.ProjectName.ToLower() == name.ToLower(), allowTracking: false);
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
            var data = await _projectRepository.GetAsync(c => 
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
            var data = await _projectRepository.Entity.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo).AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectCode == projectCode);

            return data.ToModel();
        }
    }
}
