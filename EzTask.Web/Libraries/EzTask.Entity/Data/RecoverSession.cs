using System;

namespace EzTask.Entity.Data
{
    public class RecoverSession : Entity<RecoverSession>
    {
        public Guid Code { get; set; }
        public DateTime ExpiredTime { get; set; }
        public int AccountId { get; set; }
        public bool IsUsed { get; set; }
        public Account Account { get; set; }
    }
}
