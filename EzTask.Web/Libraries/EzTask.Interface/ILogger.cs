using EzTask.Interface.SharedData;

namespace EzTask.Interface
{
    public interface ILogger
    {
        ILogEntity LogEntity { get; set; }
        void WriteInfo();
        void WriteDebug();
        void WriteError();
    }
}
