using EzTask.Interface;
using EzTask.Interface.SharedData;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class ToDoListBusiness : BusinessCore
    {
        private readonly IAccountContext _accountContext;

        public ToDoListBusiness(UnitOfWork unitOfWork, IAccountContext accountContext) : base(unitOfWork)
        {
            _accountContext = accountContext;
        }

        /// <summary>
        /// Get todo list
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<PagingModel<ToDoItemModel>> GetToDoList(int accountId, int currentPage = 1, int pageSize = 5)
        {
            var totalRecord = await UnitOfWork.TodoItemRepository
                                   .CountAsync(c => c.Owner == accountId);

            var data = UnitOfWork.TodoItemRepository
                      .GetPaging(c => c.Owner == accountId, c => c.UpdatedDate, 
                                OrderType.DESC, currentPage, pageSize, false);

            if (data.Any())
            {
                var model = data.ToModels();

                foreach(var item in model)
                {
                    item.TimeLeft = Convert.ToInt32((item.CompleteOn - DateTime.Now.Date).TotalDays);

                }

                return PagingModel<ToDoItemModel>.CreatePager(model,
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

            var data = await UnitOfWork.TodoItemRepository.GetAsync(c=>c.Id == id, false);

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
        /// Delete todoitem list
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> Deletes(int[] ids)
        {
            ResultModel<bool> result = new ResultModel<bool>();

            var dataRange = await UnitOfWork
                .TodoItemRepository.GetManyAsync(c => ids.Contains(c.Id), false);

            if (dataRange.Any())
            {
                var firstItem = dataRange.FirstOrDefault();

                if (_accountContext.AccountId == firstItem.Owner)
                {
                    UnitOfWork.TodoItemRepository.DeleteRange(dataRange);

                    int iResult = await UnitOfWork.CommitAsync();

                    if (iResult > 0)
                    {
                        result.Data = true;
                        result.Status = ActionStatus.Ok;
                    }
                }
                else
                {
                    result.Status = ActionStatus.UnAuthorized;
                }
            }
            else
            {
                // do nothing
                result.Status = ActionStatus.NotFound;
            }

            return result;
        }
    }
}
