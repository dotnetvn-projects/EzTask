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
    public class TaskBusiness : BusinessCore
    {
        private readonly PhaseBusiness _phase;
        private readonly AccountBusiness _account;
        public TaskBusiness(UnitOfWork unitOfWork,
            PhaseBusiness phase, AccountBusiness account) : base(unitOfWork)
        {
            _phase = phase;
            _account = account;
        }

        #region Task

        public async Task<ResultModel<TaskItemModel>> GetTask(int id)
        {
            ResultModel<TaskItemModel> result = new ResultModel<TaskItemModel>();
            TaskItem data = await UnitOfWork.TaskRepository.Entity
                         .Include(c => c.Project)
                         .Include(c => c.Member)
                         .Include(c => c.Assignee)
                         .Include(c => c.Phase)
                         .AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            result.Data = data.ToModel();
            return result;
        }

        public async Task<ResultModel<string>> GetTaskCode(int id)
        {
            ResultModel<string> result = new ResultModel<string>();
            string code = await UnitOfWork.TaskRepository.Entity
                         .AsNoTracking().Where(c=>c.Id == id)
                         .Select(c => c.TaskCode).FirstOrDefaultAsync();

            result.Data = code;
            return result;
        }

        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResultModel<TaskItemModel>> SaveTask(TaskItemModel model)
        {
            ResultModel<TaskItemModel> result = new ResultModel<TaskItemModel>();

            TaskItem task = model.ToEntity();

            //reset navigate object
            task.Phase = null;
            task.Project = null;
            task.Assignee = null;
            task.Member = null;
            task.UpdatedDate = DateTime.Now;
            if (task.AssigneeId == 0)
            {
                task.AssigneeId = null;
            }

            if (task.Id < 1)
            {
                task.TaskCode = string.Empty;
                task.CreatedDate = DateTime.Now;
                task.PercentCompleted = 5;
                UnitOfWork.TaskRepository.Add(task);
            }
            else
            {
                UnitOfWork.TaskRepository.Update(task);
            }


            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                if (string.IsNullOrEmpty(task.TaskCode))
                {
                    task.TaskCode = CreateCode("T", task.Id);
                    int updatedRecord = await UnitOfWork.CommitAsync();
                }

                result.Status = ActionStatus.Ok;
                result.Data = task.ToModel();
            }

            return result;
        }

        /// <summary>
        /// Count task by phase
        /// </summary>
        /// <param name="phaseId"></param>
        /// <returns></returns>
        public async Task<int> CountByPhase(int phaseId, int projectId)
        {
            var totalTask = await UnitOfWork.TaskRepository.Entity.CountAsync(c => c.PhaseId == phaseId
            && c.ProjectId == projectId);
            return totalTask;
        }

        /// <summary>
        /// Count task by project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<int> CountByProject(int projectId)
        {
            var totalTask = await UnitOfWork.TaskRepository.Entity.CountAsync(c => c.ProjectId == projectId);
            return totalTask;
        }

        /// <summary>
        /// Count task by member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<int> CountByMember(int memberId, int projectId)
        {
            var totalTask = await UnitOfWork.TaskRepository.Entity.CountAsync(c => c.MemberId == memberId
            && c.ProjectId == projectId);
            return totalTask;
        }

        /// <summary>
        /// Delete tasks
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> DeleteTask(int[] ids)
        {
            ResultModel<bool> result = new ResultModel<bool>
            {
                Data = true
            };

            IEnumerable<TaskItem> data = await UnitOfWork.TaskRepository.GetManyAsync(c => ids.Contains(c.Id));
            await DeleteTasks(result, data);

            return result;
        }

        /// <summary>
        /// Delete tasks by projectid
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> DeleteTask(int projectId)
        {
            ResultModel<bool> result = new ResultModel<bool>
            {
                Data = true
            };

            IEnumerable<TaskItem> data = await UnitOfWork.TaskRepository.GetManyAsync(c => c.ProjectId == projectId);
            await DeleteTasks(result, data);
            return result;
        }

        /// <summary>
        /// Delete tasks by projectid and phaseid
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> DeleteTask(int projectId, int phaseId)
        {
            ResultModel<bool> result = new ResultModel<bool>
            {
                Data = true
            };

            IEnumerable<TaskItem> data = await UnitOfWork.TaskRepository.GetManyAsync(c =>
                                        c.ProjectId == projectId && c.PhaseId == phaseId);
            await DeleteTasks(result, data);
            return result;
        }

        /// <summary>
        /// Get task list
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskItemModel>> GetTasks(int projectId,
            int phaseId,
            int page = 1,
            int pageSize = 9999)
        {

            List<TaskItem> data = await UnitOfWork.TaskRepository.Entity
                        .Include(c => c.Project)
                        .Include(c => c.Member)
                        .Include(c => c.Assignee)
                        .Include(c => c.Phase)
                        .AsNoTracking()
                        .Where(c => c.ProjectId == projectId && c.PhaseId == phaseId)
                        .Skip(pageSize * page - pageSize).Take(pageSize)
                        .Select(x => new TaskItem
                        {   
                            Phase = new Phase { PhaseName = x.Phase.PhaseName, Id = x.Phase.Id, IsDefault = x.Phase.IsDefault },
                            Attachments = x.Attachments.Any() ?
                                new List<Attachment>
                                {
                                    new Attachment
                                    {
                                        Id =x.Attachments.FirstOrDefault().Id
                                    }
                                } : new List<Attachment>(),
                            AssigneeId = x.AssigneeId,
                            CreatedDate = x.CreatedDate,
                            UpdatedDate = x.UpdatedDate,
                            MemberId = x.MemberId,
                            PhaseId = x.PhaseId,
                            Priority = x.Priority,
                            ProjectId = x.ProjectId,
                            Status = x.Status,
                            TaskCode = x.TaskCode,
                            TaskTitle = x.TaskTitle,
                            PercentCompleted = x.PercentCompleted,
                            Id = x.Id,

                            Member = new Account
                            {
                                AccountInfo = new AccountInfo
                                {
                                    DisplayName = x.Member.AccountInfo.DisplayName
                                },
                                Id = x.Member.Id
                            },
                            Assignee = new Account
                            {
                                AccountInfo = new AccountInfo
                                {
                                    DisplayName = x.Assignee != null ?
                                        x.Assignee.AccountInfo.DisplayName : "Non Assigned"
                                }
                            }
                        }).ToListAsync();

            IEnumerable<TaskItemModel> model = data.ToModels();

            return model;
        }

        /// <summary>
        /// Assign task to user
        /// </summary>
        /// <param name="taskids"></param>
        /// <param name="accountId"></param>
        public async Task AssignTask(int[] taskids, int accountId)
        {
            var tasks = await UnitOfWork.TaskRepository.GetManyAsync(c => taskids.Contains(c.Id), false);
            foreach (var task in tasks)
            {
                task.AssigneeId = accountId == 0 ? null : (int?)accountId;

                UnitOfWork.TaskRepository.Update(task);
            }
            var iResult = await UnitOfWork.CommitAsync();
        }

        #endregion Task

        #region Attachment

        /// <summary>
        /// Get attachment list
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AttachmentModel>> GetAttachments(int taskId)
        {
            List<Attachment> iResult = await UnitOfWork.AttachRepository.Entity.Include(c => c.Task).
                Include(c => c.User).ThenInclude(c => c.AccountInfo).AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.AddedDate).ToListAsync();

            return iResult.ToModels();
        }

        /// <summary>
        /// Get attachment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AttachmentModel> GetAttachment(int id)
        {
            Attachment iResult = await UnitOfWork.AttachRepository.GetByIdAsync(id);

            return iResult.ToModel();
        }

        /// <summary>
        /// delete attachment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAttachment(int id)
        {
            UnitOfWork.AttachRepository.Delete(id);
            await UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Save an attachment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<AttachmentModel>> SaveAttachment(AttachmentModel model)
        {
            ResultModel<AttachmentModel> result = new ResultModel<AttachmentModel>();

            Attachment entity = model.ToEntity();
            byte[] file = model.FileData;

            if (entity.Id <= 0)
            {
                entity.AddedDate = DateTime.Now;
            }
            entity.Task = null;
            entity.User = null;

            UnitOfWork.AttachRepository.Add(entity);
            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = entity.ToModel();
            }

            return result;
        }

        #endregion

        #region History

        /// <summary>
        /// Load history detail
        /// </summary>
        /// <param name="historyId"></param>
        /// <returns></returns>
        public async Task<ResultModel<TaskHistoryModel>> LoadHistoryDetail(int historyId)
        {
            ResultModel<TaskHistoryModel> result = new ResultModel<TaskHistoryModel>();
            TaskHistory data = await UnitOfWork.TaskHistoryRepository.Entity.AsNoTracking()
                                .Include(c => c.Task).FirstOrDefaultAsync(c => c.Id == historyId);
            result.Data = data.ToModel();
            return result;
        }

        /// <summary>
        /// Save history
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<TaskHistoryModel>> SaveHistory(int taskId, string title, string updateInfo, int accountId)
        {
            ResultModel<TaskHistoryModel> result = new ResultModel<TaskHistoryModel>();
            var model = new TaskHistoryModel
            {
                Content = updateInfo,
                Task = new TaskItemModel { TaskId = taskId },
                User = new AccountModel { AccountId = accountId },
                Title = title,
                UpdatedDate = DateTime.Now
            };

            TaskHistory entity = model.ToEntity();

            if (entity.Id <= 0)
            {
                entity.UpdatedDate = DateTime.Now;
            }
            entity.Task = null;
            entity.User = null;

            UnitOfWork.TaskHistoryRepository.Add(entity);
            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = entity.ToModel();
            }

            return result;
        }

        /// <summary>
        /// Get task history
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskHistoryModel>> GetHistoryList(int taskId, int accountId)
        {
            List<TaskHistory> iResult = await UnitOfWork.TaskHistoryRepository.Entity.Include(c => c.Task).
                Include(c => c.User).ThenInclude(c => c.AccountInfo).AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.UpdatedDate).ToListAsync();

            return iResult.ToModels();
        }

        /// <summary>
        /// Compare changes of old and new task
        /// </summary>
        /// <param name="newData"></param>
        /// <param name="oldData"></param>
        /// <returns></returns>
        public async Task<string> CompareChangesAsync(TaskItemModel newData, TaskItemModel oldData)
        {
            if (newData.TaskId != oldData.TaskId)
            {
                return string.Empty;
            }

            string content = string.Empty;
            if (newData.TaskTitle != oldData.TaskTitle)
            {
                content += FormartHistoryContent("Title", oldData.TaskTitle, newData.TaskTitle);
            }
            if (newData.TaskDetail != oldData.TaskDetail)
            {
                content += FormartHistoryContent("Detail", oldData.TaskDetail, newData.TaskDetail);
            }
            if (newData.Phase.Id != oldData.Phase.Id)
            {
                string oldItem = "Open Features";
                string newItem = "Open Features";
                if (newData.Phase.Id > 0)
                {
                    var phase = await _phase.GetPhaseById(newData.Phase.Id);
                    newItem = phase.PhaseName;
                }
                if (oldData.Phase.Id > 0)
                {
                    var phase = await _phase.GetPhaseById(oldData.Phase.Id);
                    oldItem = phase.PhaseName;
                }
                content += FormartHistoryContent("Phase", oldItem, newItem);
            }
            if (newData.Assignee.AccountId != oldData.Assignee.AccountId)
            {
                string oldItem = "Non-Assigned";
                string newItem = "Non-Assigned";
                if (newData.Assignee.AccountId > 0)
                {
                    var account = await _account.GetAccountInfo(newData.Assignee.AccountId);
                    newItem = account.DisplayName;
                }
                if (oldData.Assignee.AccountId > 0)
                {
                    var account = await _account.GetAccountInfo(oldData.Assignee.AccountId);
                    oldItem = account.DisplayName;
                }
                content += FormartHistoryContent("Assignee", oldItem, newItem);
            }
            if (newData.Priority != oldData.Priority)
            {
                string oldItem = oldData.Priority.ToString();
                string newItem = newData.Priority.ToString();
                content += FormartHistoryContent("Priority", oldItem, newItem);
            }
            if (newData.Status != oldData.Status)
            {
                string oldItem = oldData.Status.ToString();
                string newItem = newData.Status.ToString();
                content += FormartHistoryContent("Status", oldItem, newItem);
            }
            if (newData.PercentCompleted != oldData.PercentCompleted)
            {
                string oldItem = oldData.PercentCompleted.ToString();
                string newItem = newData.PercentCompleted.ToString();
                content += FormartHistoryContent("Percent Completed", oldItem, newItem);
            }

            return content;
        }
        #endregion

        #region Private

        private async Task DeleteTasks(ResultModel<bool> result, IEnumerable<TaskItem> data)
        {
            if (!data.Any())
            {
                result.Status = ActionStatus.NotFound;
            }
            else
            {
                foreach (TaskItem task in data)
                {
                    IEnumerable<TaskHistory> history = UnitOfWork.TaskHistoryRepository.GetMany(c => c.TaskId == task.Id);
                    if (history.Any())
                    {
                        UnitOfWork.TaskHistoryRepository.DeleteRange(history);
                    }

                    IEnumerable<Attachment> attachment = UnitOfWork.AttachRepository.GetMany(c => c.TaskId == task.Id);
                    if (attachment.Any())
                    {
                        UnitOfWork.AttachRepository.DeleteRange(attachment);
                    }
                }

                UnitOfWork.TaskRepository.DeleteRange(data);
                int iResult = await UnitOfWork.CommitAsync();
                if (iResult > 0)
                {
                    result.Status = ActionStatus.Ok;
                }
            }
        }

        private string FormartHistoryContent(string field, string oldData, string newData)
        {
            if(field == "Detail")
            {
                string content = $"<p><b>{field}</b>:<br/>";
                content += $"Old data:<br/> <small style=\"word-wrap: break-word;\">{oldData}</small><br/>";
                content += $"New data:<br/> <small style=\"word-wrap: break-word;\" class='text-danger'>{newData}</small><br/>";
                return content;
            }
            return $"<p><b>{field}</b>:<br/><small>{oldData} => <span style=\"color:red;\">{newData}</span></small></p>";
        }

        #endregion
    }
}