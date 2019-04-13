using EzTask.Business;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzTask.Modules.Dashboard.ViewComponents
{
    // http://jasonwatmore.com/post/2016/08/23/angular-2-pagination-example-with-logic-like-google
    public class TodoListViewComponent: ViewComponent
    {
        private readonly EzTaskBusiness _ezTask;

        public TodoListViewComponent(EzTaskBusiness eztask)
        {
            _ezTask = eztask;
        }

        public async Task<IViewComponentResult> InvokeAsync(int currentPage = 1, int pageSize = 5)
        {
            int accountId = Context.CurrentAccount.AccountId;
            var model = await _ezTask.ToDoList.GetToDoList(accountId, currentPage, pageSize);
            return View(model);
        }
    }
}
