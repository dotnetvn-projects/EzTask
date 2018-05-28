using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using EzTask.Framework.Infrastructures;
using EzTask.Models;
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
        public async Task<ResultModel> CreateTask(TaskItemModel model)
        {
            ResultModel result = new ResultModel();

            var task = model.ToEntity();

            DbContext.Tasks.Add(task);
            var iResult = await DbContext.SaveChangesAsync();

            if (iResult > 0)
            {
                if (string.IsNullOrEmpty(task.TaskCode))
                {
                    task.TaskCode = CreateCode("T", task.Id);
                    var updatedRecord = await DbContext.SaveChangesAsync();
                }

                result.Status = ActionStatus.Ok;
                result.Value = task.ToModel();
            }

            result.Status = ActionStatus.Failed;
            return result;
        }

        /// <summary>
        /// Get task list
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskItemModel>> GetTasks(int projectId,
            int page, int pageSize)
        {

            var data = await DbContext.Tasks.Include(c => c.Project)
                                   .Include(c => c.Member)
                                   .Include(c => c.Assignee)
                                   .Include(c => c.Phrase)
                                   .AsNoTracking()
                                   .Where(c => c.ProjectId == projectId)
                                   .Skip(pageSize * page - pageSize).Take(pageSize)
                                   .ToListAsync();

            var model = data.ToModels();
            return model;
        }
    }
}
