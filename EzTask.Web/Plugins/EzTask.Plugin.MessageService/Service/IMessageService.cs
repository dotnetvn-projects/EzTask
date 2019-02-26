using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Plugin.MessageService
{
    public interface IMessageService<T>
    {
        void Delivery(T message);
    }
}
