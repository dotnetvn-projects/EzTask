using EzTask.Framework.Data;
using System.Collections.Concurrent;

namespace EzTask.Framework.Objects
{
    public class LogQueue
    {
        public static ConcurrentQueue<LogData> _queue = 
            new ConcurrentQueue<LogData>();
        
        public static void AddQueue(LogData logEntity)
        {
            _queue.Enqueue(logEntity);
        }
    }
}
