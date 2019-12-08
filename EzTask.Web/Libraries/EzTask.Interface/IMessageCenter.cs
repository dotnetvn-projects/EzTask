using EzTask.Interface.SharedData;

namespace EzTask.Interface
{
    public interface IMessageCenter
    {
        void Push(IMessage message);
    }
}
