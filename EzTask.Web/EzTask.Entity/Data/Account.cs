using System;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Entity.Data
{
    public class Account:BaseEntity<Account>
    {
        public int? ManageAccountId { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Int16 AccountStatus { get; set; }
        public string Comment { get; set; }

        public AccountInfo AccountInfo { get; set; }
        public Account ManageAccount { get; set; }

    }
}
