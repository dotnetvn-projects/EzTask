using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interfaces
{
    public interface IMessageService<T>
    {
        ConcurrentQueue<T> MessageQueue { get; }
        void Delivery();
        void Enqueue(T message);
        void Start();
        void Stop();
    }
}
