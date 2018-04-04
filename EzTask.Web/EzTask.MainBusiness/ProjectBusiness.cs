﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
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

            EzTaskDbContext.Projects.Add(project);
            var insertedRecord = await EzTaskDbContext.SaveChangesAsync();

            if (insertedRecord > 0)
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
        /// Get project list for a specific user
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Project>> GetProjects(int ownerId)
        {
            return await EzTaskDbContext.Projects.Include(c => c.Account)
                .ThenInclude(c => c.AccountInfo)
                .AsNoTracking()
                .Where(c => c.Owner == ownerId)
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
