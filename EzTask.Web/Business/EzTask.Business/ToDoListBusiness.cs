using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Framework.Infrastructures;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Repository;

namespace EzTask.Business
{
    public class ToDoListBusiness : BusinessCore
    {
        public ToDoListBusiness(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Get todo list
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<PagingModel<ToDoItemModel>> GetToDoList(int accountId, int currentPage = 1, int pageSize = 5)
        {
            var totalRecord = await UnitOfWork.TodoItemRepository.CountAsync(c => c.Owner == accountId);
            var data = await UnitOfWork.TodoItemRepository.GetPagingAsync(c => c.Owner == accountId,
                        currentPage, pageSize, false);

            if(data.Any())
            {
                return PagingModel<ToDoItemModel>.CreatePager(data.ToModels(),
                            totalRecord, pageSize, currentPage);
            }
            return new PagingModel<ToDoItemModel>();
        }

        /// <summary>
        /// Get todo item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultModel<ToDoItemModel>> GetTodoItem(int id)
        {
            ResultModel<ToDoItemModel> result = new ResultModel<ToDoItemModel>();
            var data = await UnitOfWork.TodoItemRepository.GetByIdAsync(id, false);
            if(data != null)
            {
                result.Data = data.ToModel();
                result.Status = ActionStatus.Ok;
            }
            return result;
        }

        /// <summary>
        /// Save a todoitem
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<ToDoItemModel>> Save(ToDoItemModel model)
        {
            ResultModel<ToDoItemModel> iResult = new ResultModel<ToDoItemModel>();
            var entity = model.ToEntity();
            entity.Account = null;

            if(entity.Id > 0)
            {
                UnitOfWork.TodoItemRepository.Update(entity);
            }
            else
            {
                UnitOfWork.TodoItemRepository.Add(entity);
            }

            var result = await UnitOfWork.CommitAsync();

            if(result > 0)
            {
                iResult.Status = ActionStatus.Ok;
                iResult.Data = entity.ToModel();
            }

            return iResult;
        }

        /// <summary>
        /// Delete a todoitem
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<ToDoItemModel>> Delete(ToDoItemModel model)
        {
            ResultModel<ToDoItemModel> result = new ResultModel<ToDoItemModel>();

            UnitOfWork.TodoItemRepository.Delete(model.Id);
            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
            }

            return result;
        }

        /// <summary>
        /// Delete todoitem list
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<ResultModel<ToDoItemModel>> Deletes(List<ToDoItemModel> models)
        {
            ResultModel<ToDoItemModel> result = new ResultModel<ToDoItemModel>();

            var ids = models.Select(c => c.Id).ToList();
            var dataRange = await UnitOfWork.TodoItemRepository.GetManyAsync(c => ids.Contains(c.Id));

            UnitOfWork.TodoItemRepository.DeleteRange(dataRange);

            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
            }

            return result;
        }
    }
}
