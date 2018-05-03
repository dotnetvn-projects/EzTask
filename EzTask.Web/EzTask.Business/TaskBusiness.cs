using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class TaskBusiness : BaseBusiness
    {
        public TaskBusiness(EzTaskDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<TaskItem> CreateTask(TaskItem item)
        {
            DbContext.Tasks.Add(item);
            var iResult =  await DbContext.SaveChangesAsync();
            if (iResult > 0)
            {
                if (string.IsNullOrEmpty(item.TaskCode))
                {
                    item.TaskCode = CreateCode("T", item.Id);
                    var updatedRecord = await DbContext.SaveChangesAsync();
                }
                return item;
            }
            return null;
        }

        /// <summary>
        /// Get task list
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskItem>> GetTasks(int projectId, 
            int page, int pageSize)
        {
           
            var data = await DbContext.Tasks.Include(c => c.Project)
                                   .Include(c => c.Member)
                                   .Include(c => c.Assignee)
                                   .AsNoTracking()
                                   .Where(c=>c.ProjectId == projectId)
                                   .Skip(pageSize * page - pageSize).Take(pageSize)
                                   .ToListAsync();
            return data;
        }
    }
}
