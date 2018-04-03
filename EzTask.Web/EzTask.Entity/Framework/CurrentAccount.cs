using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Framework
{
    public class CurrentAccount
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }

        public static CurrentAccount Create(string accountId, string accountName)
        {
            return new CurrentAccount
            {
                AccountId = accountId,
                AccountName = accountName
            };
        }
    }
}
