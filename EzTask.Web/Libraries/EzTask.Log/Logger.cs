using EzTask.Interface;
using EzTask.Interface.SharedData;
using log4net;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace EzTask.Log
{
    public class Logger : ILogger
    {

        private static readonly string LOG_CONFIG_FILE = @"log4net.config";

        private static readonly log4net.ILog _log = GetLogger(typeof(Logger));

        public ILogEntity LogEntity { get; set; }

        public Logger()
        {
            SetLog4NetConfiguration();
        }

        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        private static void SetLog4NetConfiguration()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(LOG_CONFIG_FILE));

            var repo = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }

        public void WriteDebug()
        {
            _log.Debug(BuildMessage(LogEntity));
        }

        public void WriteError()
        {
            _log.Error(BuildMessage(LogEntity), LogEntity.Exception);
        }

        public void WriteInfo()
        {
            _log.Info(BuildMessage(LogEntity));
        }

        private static string BuildMessage(ILogEntity logEntity)
        {
            return string.Format("[{0}][{1}] {2}", logEntity.Function, logEntity.AccountName, logEntity.Message);
        }
    }
}
