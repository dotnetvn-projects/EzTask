using EzTask.Framework.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Web.Framework.HttpContext
{
    public class AccountContext
    {
        private CurrentAccount _currentAccount
        {
            get
            {
                SessionManager sessionManager = new SessionManager();
                var currentAccount = sessionManager.GetObject<CurrentAccount>(AppKey.Account);
                return currentAccount;
            }
        }

        /// <summary>
        /// Account Id
        /// </summary>
        public int AccountId
        {
            get { return int.Parse(_currentAccount.AccountId); }
        }

        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName
        {
            get { return _currentAccount.NickName; }
        }
    }
}
