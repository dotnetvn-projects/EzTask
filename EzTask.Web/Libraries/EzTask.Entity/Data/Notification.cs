using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Data
{
    public class Notification : Entity<Notification>
    {
        public int AccountId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public short Context { get; set; }
        public string RefData { get; set; }
        public bool HasViewed { get; set; }

        public Account Account { get; set; }
    }
}
