using EzTask.Interface.SharedData;
using System;

namespace EzTask.Log
{
    public class LogEntity : ILogEntity
    {
        public string Message { get; private set; }
        public string AccountName { get; private set; }
        public string Function { get; private set; }
        public Exception Exception { get; private set; }

        public static LogEntity Create(string accountName, string message,
            string function, Exception exception = null)
        {
            return new LogEntity
            {
                AccountName = accountName,
                Exception = exception,
                Function = function,
                Message = message
            };
        }
    }
}
