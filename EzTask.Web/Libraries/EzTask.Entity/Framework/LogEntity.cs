using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Framework
{
    public class LogEntity
    {
        public string Message { get; set; }
        public string EventName { get; set; }
        public string AccountName { get; set; }
        public string Function { get; set; }
        public string Source { get; set; }
        public Exception Exception { get; set; }
    }
}
