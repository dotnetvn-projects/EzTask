﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EzTask.Framework.Web.Filters;
using EzTask.Management.Infrastructures;
using EzTask.Management.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Management.Controllers
{
    [TypeFilter(typeof(AuthorizeFilter))]
    public class UserManageController : EzTaskController
    {
        public UserManageController(IServiceProvider serviceProvider, IMapper mapper) :
            base(serviceProvider, mapper)
        {
        }

        #region View

        [Route("user/list.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("user/create.html")]
        public IActionResult Create()
        {
            return View();
        }

        #endregion

        #region Http Action

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
        [Route("user/create.html")]
        public IActionResult Create(AccountModel model)
        {
            return View();
        }

        #endregion

        #region Non Action

        #endregion
    }
}