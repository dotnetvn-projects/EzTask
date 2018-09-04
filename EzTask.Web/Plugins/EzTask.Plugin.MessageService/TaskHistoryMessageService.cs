using EzTask.Business;
using EzTask.Interfaces;
using EzTask.Plugin.MessageService.Data;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace EzTask.Plugin.MessageService
{
    public class TaskHistoryMessageService : IMessageService<TaskHistoryMessage>
    {
        private CancellationTokenSource wtoken;
        private TaskBusiness business;

        public ConcurrentQueue<TaskHistoryMessage> MessageQueue
        {
            get;
            private set;
        }

        public void Delivery()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    while (MessageQueue.Count != 0)
                    {
                        TaskHistoryMessage message;
                        MessageQueue.TryDequeue(out message);

                    }
                    Task.Delay(3000, wtoken.Token);
                }
            }, wtoken.Token);
        }

        public void Enqueue(TaskHistoryMessage message)
        {
            MessageQueue.Enqueue(message);
        }

        public void Start()
        {
            wtoken = new CancellationTokenSource();
            Delivery();
        }

        public void Stop()
        {
            wtoken.Cancel();
        }
    }
}
