namespace EzTask.Plugin.MessageService
{
    public interface IMessageService<T>
    {
        void Delivery(T message);
    }
}
