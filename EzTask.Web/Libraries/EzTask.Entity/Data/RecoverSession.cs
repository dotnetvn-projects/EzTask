using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Data
{
    public class RecoverSession : Entity<RecoverSession>
    {
        public Guid Id { get; set; }
        public DateTime ExpiredTime { get; set; }
        public int AccountId { get; set; }
        public bool IsUsed { get; set; }
        public Account Account { get; set; }
    }
}
