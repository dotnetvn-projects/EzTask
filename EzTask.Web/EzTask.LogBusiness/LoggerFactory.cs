using EzTask.Entity.Framework;
using EzTask.Interfaces;

namespace EzTask.LogBusiness
{
    public class LoggerFactory
    {
        public static ILogger GetLogger(LogType logType)
        {
            ILogger logger = null;
            switch(logType)
            {
                case LogType.Error:
                    logger = new LogError();
                    break;
                case LogType.Info:
                    logger = new LogInfor();
                    break;
                case LogType.LogToDb:
                    logger = new LogDb();
                    break;
            }
            return logger;
        }
    }
}
