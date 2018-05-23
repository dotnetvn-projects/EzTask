﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using EzTask.Framework.Infrastructures;
using EzTask.Models;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class ProjectBusiness : BaseBusiness
    {
        public ProjectBusiness(EzTaskDbContext dbContext) :
            base(dbContext)
        {
        }

        /// <summary>
        /// Save project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ProjectModel> Save(ProjectModel model)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var project = model.ToEntity();

                    if (project.Id < 1)
                    {
                        project.CreatedDate = DateTime.Now;
                    }

                    project.UpdatedDate = DateTime.Now;

                    if (project.Id < 1)
                    {
                        DbContext.Projects.Add(project);
                    }
                    else
                    {
                        DbContext.Attach(project);
                        DbContext.Entry(project).State = EntityState.Modified;
                    }

                    var iResult = await DbContext.SaveChangesAsync();

                    if (iResult > 0)
                    {
                        if (string.IsNullOrEmpty(project.ProjectCode))
                        {
                            project.ProjectCode = CreateCode("EzT", project.Id);
                            iResult = await DbContext.SaveChangesAsync();
                            if (iResult > 0)
                            {
                                //create feature in Phrase table as default
                                var feature = new Phrase
                                {
                                    PhraseName = "Features",
                                    ProjectId = project.Id,
                                    Status = (int)PhraseStatus.Open
                                };
                                DbContext.Phrases.Add(feature);
                                await DbContext.SaveChangesAsync();
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
        public async Task<ActionStatus> Delete(string projectCode)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var project = await GetProject(projectCode);
                    if (project != null)
                    {
                        //Remove member
                        var memberList = await DbContext.ProjectMembers.Where(c => c.ProjectId == project.ProjectId).ToListAsync();
                        DbContext.ProjectMembers.RemoveRange(memberList);

                        //remove phrase
                        var phrases = await DbContext.Phrases.Where(c => c.ProjectId == project.ProjectId).ToListAsync();
                        DbContext.Phrases.RemoveRange(phrases);

                        //remove project
                        DbContext.Projects.Remove(project.ToEntity());

                        await DbContext.SaveChangesAsync();
                    }

                    transaction.Commit();
                    return ActionStatus.Ok;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return ActionStatus.Failed;
                }
            }
        }

        /// <summary>
        /// Get project list for a specific user
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectModel>> GetProjects(int ownerId)
        {
            var data =  await DbContext.Projects.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .AsNoTracking()
                .Where(c => c.Owner == ownerId).OrderBy(c => c.Status)
                .ToListAsync();

            return data.ToModels();
        }

        /// <summary>
        ///  Get project
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProject(string projectCode)
        {
            var data = await DbContext.Projects.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectCode == projectCode);
            return data.ToModel();
        }

        /// <summary>
        ///  Get project by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectByName(string name)
        {
            var data = await DbContext.Projects.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectName.ToLower() == name.ToLower());
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
            return await DbContext.Projects.AsNoTracking()
                .AnyAsync(c => c.ProjectName.ToLower() == name.ToLower() && c.Id != id);
        }


        /// <summary>
        ///  Get project detail
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectDetail(string projectCode)
        {
            var data = await DbContext.Projects.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo).AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectCode == projectCode);

            return data.ToModel();
        }
    }
}
