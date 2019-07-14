using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Data
{
    public class RecoverSession
    {
        public Guid Id { get; set; }
        public DateTime ExpiredTime { get; set; }
        public int AccountId { get; set; }
    }
}
