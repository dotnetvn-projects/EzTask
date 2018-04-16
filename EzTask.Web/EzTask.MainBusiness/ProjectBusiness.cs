using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using Microsoft.EntityFrameworkCore;

namespace EzTask.MainBusiness
{
    public class ProjectBusiness : BaseBusiness
    {
        public ProjectBusiness(EzTaskDbContext ezTaskDbContext) : 
            base(ezTaskDbContext)
        {
        }

        /// <summary>
        /// Save project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task<Project> Save(Project project)
        {
            if (project.Id < 1)
            {
                project.CreatedDate = DateTime.Now;
            }
            project.UpdatedDate = DateTime.Now;
            if (project.Id < 1)
            {
                EzTaskDbContext.Projects.Add(project);
            }
            else
            {
                EzTaskDbContext.Attach(project);
                EzTaskDbContext.Entry(project).State = EntityState.Modified;
            }
            var records = await EzTaskDbContext.SaveChangesAsync();

            if (records > 0)
            {
                if (string.IsNullOrEmpty(project.ProjectCode))
                {
                    project.ProjectCode = GenerateProjectCode(project.Id);
                    var updatedRecord = await EzTaskDbContext.SaveChangesAsync();
                }

               return project;
            }

            return null;
        }

        /// <summary>
        /// Remove project
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<ActionStatus> Delete(string projectCode)
        {
            using (var dbContextTransaction = EzTaskDbContext.Database.BeginTransaction())
            {
                try
                {
                    var project = await GetProject(projectCode);
                    if (project != null)
                    {
                        var memberList = await EzTaskDbContext.ProjectMembers.Where(c => c.ProjectId 
                        == project.Id).ToListAsync();

                        EzTaskDbContext.ProjectMembers.RemoveRange(memberList);

                        EzTaskDbContext.Projects.Remove(project);

                        await EzTaskDbContext.SaveChangesAsync();
                    }

                    dbContextTransaction.Commit();
                    return ActionStatus.Ok;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return ActionStatus.Failed;
                }
            }            
        }

        /// <summary>
        /// Get project list for a specific user
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Project>> GetProjects(int ownerId)
        {
            return await EzTaskDbContext.Projects.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)        
                .AsNoTracking()
                .Where(c => c.Owner == ownerId).OrderBy(c=>c.Status)
                .ToListAsync();
        }

        /// <summary>
        ///  Get project
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<Project> GetProject(string projectCode)
        {
            return await EzTaskDbContext.Projects.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectCode == projectCode);
        }

        /// <summary>
        ///  Get project by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Project> GetProjectByName(string name)
        {
            return await EzTaskDbContext.Projects.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectName.ToLower() == name.ToLower());
        }

        /// <summary>
        /// Check the project is dupplicated or not
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns>True if it is, False if it is not</returns>
        public async Task<bool> IsDupplicated(string name, int id)
        {
            return await EzTaskDbContext.Projects.AsNoTracking()
                .AnyAsync(c => c.ProjectName.ToLower() == name.ToLower() && c.Id != id);
        }


        /// <summary>
        ///  Get project detail
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public async Task<Project> GetProjectDetail(string projectCode)
        {
            return await EzTaskDbContext.Projects.Include(c=>c.Account)
                .ThenInclude(c=>c.AccountInfo).AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProjectCode == projectCode);
        }

        /// <summary>
        /// Generate Project code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GenerateProjectCode(int id)
        {
            if(id<100 && id > 9)
            {
                return "EzTask#0" + id;
            }
            else if(id < 10)
            {
               return "EzTask#00" + id;
            }

            return "EzTask#" + id;
        }
    }
}
