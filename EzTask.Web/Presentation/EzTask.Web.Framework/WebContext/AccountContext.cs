﻿using EzTask.Framework.GlobalData;
using EzTask.Interface;
using EzTask.Interface.SharedData;
using System;

namespace EzTask.Web.Framework.WebContext
{
    public class AccountContext : IAccountContext
    {
        private SessionManager _sessionManager;

        public AccountContext(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public void Set(IAccountInfo currentAccount)
        {
            _sessionManager.SetObject(SessionKey.Account, currentAccount);
        }

        private CurrentAccount _currentAccount
        {
            get
            {
                var currentAccount = _sessionManager.GetObject<CurrentAccount>(SessionKey.Account);
                if (currentAccount == null)
                    currentAccount = new CurrentAccount();
                return currentAccount;
            }
        }

        public bool IsLogined
        {
            get
            {
                return AccountId != 0;
            }
        }

        /// <summary>
        /// Account Id
        /// </summary>
        public int AccountId
        {
            get { return _currentAccount.AccountId; }
        }

        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName
        {
            get { return _currentAccount.DisplayName; }
        }

        /// <summary>
        /// AccountName
        /// </summary>
        public string AccountName
        {
            get { return _currentAccount.AccountName; }
        }

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime CreatedDate
        {
            get { return _currentAccount.CreatedDate; }
        }

        /// <summary>
        /// JobTitle
        /// </summary>
        public string JobTitle
        {
            get { return _currentAccount.JobTitle; }
        }
    }
}
