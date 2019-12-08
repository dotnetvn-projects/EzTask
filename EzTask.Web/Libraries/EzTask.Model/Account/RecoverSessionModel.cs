using System;

namespace EzTask.Model
{
    public class RecoverSessionModel : BaseModel
    {
        public Guid Code { get; set; }
        public DateTime ExpiredTime { get; set; }

        public AccountModel Account { get; set; }
    }
}
