using EzTask.Framework.Common;
using EzTask.Framework.Data;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Dashboard.ViewModels;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.Data;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Dashboard.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class HomeController : BaseController
    {
        public HomeController(IServiceProvider serviceProvider) : 
            base(serviceProvider)
        {
        }
    
        public IActionResult Index()
        {
            return View();
        }

        #region ToDoList

        /// <summary>
        /// create or edit form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("dashboard/todolist/generateview.html")]
        public async Task<IActionResult> GenerateAddOrEditTodoItemForm(int itemId = 0)
        {
            var model = new ToDoItemViewModel
            {
                AccountId = Context.CurrentAccount.AccountId
            };

            if (itemId > 0)
            {
                var result = await EzTask.ToDoList.GetTodoItem(itemId);
                if (result.Status == ActionStatus.Ok)
                {
                    model.CompleteOn = result.Data.CompleteOn.ToDateString();
                    model.Status = result.Data.Status.ToInt16<ToDoItemStatus>();
                    model.Priority = result.Data.Priority.ToInt16<ToDoItemPriority>();
                    model.Id = result.Data.Id;
                    model.Title = result.Data.Title;
                }
                else
                {
                    return NotFound("Item doesn't exist!");
                }
            }

            model.PriorityList = StaticResources.BuildToDoItemPrioritySelectList(model.Priority);
            model.StatusList = StaticResources.BuildToDoItemStatusSelectList(model.Status);

            return PartialView("_CreateOrEditTotoItem", model);
        }

        /// <summary>
        /// add or update todo item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("dashboard/todolist/save.html")]
        public async Task<IActionResult> AddOrEditToDoItem(ToDoItemViewModel viewModel)
        {
            var model = new ToDoItemModel
            {
                Title = viewModel.Title,
                Id = viewModel.Id,
                Account = new AccountModel { AccountId = Context.CurrentAccount.AccountId },
                CompleteOn = DateTimeUtilities.ParseFromString(viewModel.CompleteOn).Value,
                Priority = viewModel.Priority.ToEnum<ToDoItemPriority>(),
                Status = viewModel.Status.ToEnum<ToDoItemStatus>()
            };

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                return BadRequest("Title cannot empty!");
            }

           
            if (model.CompleteOn < DateTime.Now)
            {
                return BadRequest(Context.GetStringResource("CompleteOnError",
                    StringResourceType.DashboardPage));
            }

            var result = await EzTask.ToDoList.Save(model);

            return Json(result);
        }

        /// <summary>
        /// load to do list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("dashboard/todo-list.html")]
        public IActionResult GetTodoList(int currentPage)
        {
            return ViewComponent("TodoList", new { currentPage });
        }

        /// <summary>
        /// remove to do list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("dashboard/remove-todo-list.html")]
        public async Task<IActionResult> RemoveTodoList(int[] items)
        {
            var result = await EzTask.ToDoList.Deletes(items);
            return Json(result);           
        }

        #endregion
    }
}
