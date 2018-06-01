using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Data
{
    public class LogData
    {
        public string Message { get; set; }
        public string EventName { get; set; }
        public string AccountName { get; set; }
        public string Function { get; set; }
        public string Source { get; set; }
        public Exception Exception { get; set; }
    }
}
