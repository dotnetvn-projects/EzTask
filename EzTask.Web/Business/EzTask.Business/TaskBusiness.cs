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
    public class TaskBusiness : BusinessCore
    {

        public TaskBusiness(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResultModel<TaskItemModel>> CreateTask(TaskItemModel model)
        {
            ResultModel<TaskItemModel> result = new ResultModel<TaskItemModel>();

            var task = model.ToEntity();
            if(task.Id < 1)
            {
                task.CreatedDate = DateTime.Now;
            }

            task.UpdatedDate = DateTime.Now;

            UnitOfWork.TaskRepository.Add(task);
            var iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                if (string.IsNullOrEmpty(task.TaskCode))
                {
                    task.TaskCode = CreateCode("T", task.Id);
                    var updatedRecord = await UnitOfWork.CommitAsync();
                }

                result.Status = ActionStatus.Ok;
                result.Data = task.ToModel();
            }

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

            var data = await UnitOfWork.TaskRepository.Entity.Include(c => c.Project)
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
