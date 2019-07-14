using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Model
{
    public class RecoverSessionModel : BaseModel
    {
        public Guid Id { get; set; }
        public DateTime ExpiredTime { get; set; }
        public int AccountId { get; set; }
    }
}
