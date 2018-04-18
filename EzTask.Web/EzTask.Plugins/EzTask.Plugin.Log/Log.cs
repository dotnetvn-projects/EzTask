using EzTask.Entity.Framework;
using EzTask.Interfaces;

namespace EzTask.Plugin.Log
{
    public class Log
    {
        public LogType LogType;

        public Log(LogType logType = LogType.Info)
        {
            LogType = logType;
        }

        public ILogger Logger { get { return LoggerFactory.GetLogger(LogType); } } 
    }
}
