using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interface
{
    public interface IMessageService<T>
    {
        void Delivery(T message);
    }
}
