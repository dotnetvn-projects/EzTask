using System;

namespace EzTask.Framework.Data
{
    public class CurrentAccount
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string NickName { get; set; }
        public string JobTitle { get; set; }
        public DateTime CreatedDate { get; set; }

        public static CurrentAccount Create(string accountId, string accountName,
            string nickName, string jobTitle, DateTime createdDate)
        {
            return new CurrentAccount
            {
                AccountId = accountId,
                AccountName = accountName,
                NickName = nickName ,
                JobTitle = jobTitle,
                CreatedDate = createdDate
            };
        }
    }
}
