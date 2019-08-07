using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interface.SharedData
{
    public interface ILogEntity
    {
        string Message { get; set; }
        Exception Exception { get; set; }
        string AccountName { get; set; }
        string Function { get; set; }
    }
}
