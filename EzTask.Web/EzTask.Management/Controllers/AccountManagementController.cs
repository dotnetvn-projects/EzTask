using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Management.Infrastructures;
using EzTask.Management.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Management.Controllers
{
    public class AccountManagementController : EzTaskController
    {
        public AccountManagementController(IServiceProvider serviceProvider, IHttpContextAccessor httpContext)
            : base(serviceProvider, httpContext) { }

        #region View

        [Route("account/list.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("account/create.html")]
        public IActionResult Create()
        {
            return View();
        }

        #endregion

        #region Http Action

        [Route("account/account-list.html")]
        public async Task<IEnumerable<AccountModel>> GetAccountList(int page=1, int pageSize=20)
        {
            IEnumerable<AccountModel> accountModels = new List<AccountModel>();
            var data = await EzTask.Account.GetAccountList(page, pageSize, AccountId);
            if(data.Any())
            {
                accountModels = data.MapToModels();
            }
            return accountModels;
        }

        [HttpPost]
        [Route("account/create.html")]
        public IActionResult Create(AccountModel model)
        {
            return View();
        }

        #endregion

        #region Non Action

        #endregion
    }
}