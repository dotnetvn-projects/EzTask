using EzTask.Framework.Common;
using EzTask.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzTask.Framework.Infrastructures
{
    public class LocalizationMapper
    {
        public Localization Data { get; set; }

        public LocalizationMapper(string filepath)
        {
            Data = XmlUtilities.Deserialize<Localization>(filepath);
        }

        public string GetCommonMessageLang(string key)
        {
            var existKey = Data.CommonMessage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetNotificationMessageLang(string key)
        {
            var existKey = Data.NotificationMessage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetErrorMessageLang(string key)
        {
            var existKey = Data.ErrorMessage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetSuccessMessageLang(string key)
        {
            var existKey = Data.SuccessMessage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetMessageTitleLang(string key)
        {
            var existKey = Data.MessageTitle.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetDashboardPageLang(string key)
        {
            var existKey = Data.DashboardPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }     

        public string GetProjectPageLang(string key)
        {
            var existKey = Data.ProjectPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }
    }
}
