using EzTask.Entity.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Values
{
    public class LogQueue
    {
        public static ConcurrentQueue<LogEntity> _queue = 
            new ConcurrentQueue<LogEntity>();
        
        public static void AddQueue(LogEntity logEntity)
        {
            _queue.Enqueue(logEntity);
        }
    }
}
