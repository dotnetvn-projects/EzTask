using EzTask.Interfaces;
using EzTask.Plugin.MessageService.Data;
using System;
using System.Collections.Concurrent;

namespace EzTask.Plugin.MessageService
{
    public class PushNotifyMessageService : IMessageService<PushMessage>
    {

        public PushNotifyMessageService()
        {

        }

        public ConcurrentQueue<PushMessage> MessageQueue => throw new NotImplementedException();

        public void Delivery()
        {
            throw new NotImplementedException();
        }

        public void Enqueue(PushMessage message)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
