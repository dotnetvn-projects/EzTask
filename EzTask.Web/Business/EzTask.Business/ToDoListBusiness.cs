using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EzTask.Framework.Infrastructures;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Model.ToDoList;
using EzTask.Repository;

namespace EzTask.Business
{
    public class ToDoListBusiness : BusinessCore
    {
        public ToDoListBusiness(UnitOfWork unitOfWork) : base(unitOfWork)
        {
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
    }
}
