using EzTask.Entity.Framework;
using EzTask.Interfaces;

namespace EzTask.LogBusiness
{
    public class LogBusiness
    {
        public LogType LogType;

        public LogBusiness(LogType logType = LogType.Info)
        {
            LogType = logType;
        }

        public ILogger Logger { get { return LoggerFactory.GetLogger(LogType); } } 


    }
}
