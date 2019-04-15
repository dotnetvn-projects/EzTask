using EzTask.Framework.Data;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;
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
        /// add new to do item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddOrEditToDoItem(ToDoItemModel model)
        {
            if(string.IsNullOrWhiteSpace(model.Title))
            {
                return BadRequest("");
            }

            if (model.CompleteOn < DateTime.Now)
            {
                return BadRequest(Context.GetStringResource("CompleteOnError", StringResourceType.DashboardPage));
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
