using EzTask.Entity.Framework;
using EzTask.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EzTask.Framework.IO;
namespace EzTask.LogBusiness
{
    internal class LogError : ILogger
    {
        private const string TEMPLATE = "ERROR {0} {1} Source: {2} Function: {3} UserAccount: {4}";
        public void Write(LogEntity entity)
        {
            var log = string.Format(TEMPLATE, DateTime.Now.ToString("yyyy/MM/dd/ HH:mm:ss"),
                entity.EventName, entity.Source, entity.Function, entity.AccountName);

            if(entity.Exception !=null)
            {
                log +="\r\n\t" + entity.Exception.ToString();
            }

            var fileName = @"Logs/"+DateTime.Now.ToString("yyyyMMdd")+".txt";
            File.AppendText(fileName, log);
        }
    }
}
