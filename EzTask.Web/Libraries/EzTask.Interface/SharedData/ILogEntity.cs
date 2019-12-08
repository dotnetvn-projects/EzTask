using System;

namespace EzTask.Interface.SharedData
{
    public interface ILogEntity
    {
        string Message { get; }
        Exception Exception { get; }
        string AccountName { get; }
        string Function { get; }
    }
}
