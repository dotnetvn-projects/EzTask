using EzTask.Interfaces;
using EzTask.Plugin.MessageService.Data;
using System;
using System.Collections.Concurrent;

namespace EzTask.Plugin.MessageService
{
    public class PushMessageService : IMessageService<PushMessage>
    {

        public PushMessageService()
        {

        }

        public ConcurrentQueue<PushMessage> MessageQueue => throw new NotImplementedException();

        public void Delivery()
        {
            throw new NotImplementedException();
        }

        public void Enqueue(PushMessage message)
        {
            MessageQueue.Enqueue(message);
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
