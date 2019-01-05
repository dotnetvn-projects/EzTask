using System;

namespace EzTask.Framework.Data
{
    public class CurrentAccount
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string JobTitle { get; set; }
        public string Language { get; set; }
        public DateTime CreatedDate { get; set; }

        public static CurrentAccount Create(int accountId, string accountName,
            string displayName, string jobTitle, string language, DateTime createdDate)
        {
            return new CurrentAccount
            {
                AccountId = accountId,
                AccountName = accountName,
                DisplayName = displayName,
                JobTitle = jobTitle,
                Language = language,
                CreatedDate = createdDate
            };
        }
    }
}
