using System;

namespace EzTask.Entity.Data
{
    public class Account : Entity<Account>
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

        public override void ResetNavigate()
        {
            AccountInfo = null;
            ManageAccount = null;
        }
    }
}
