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

        public string GetCommonLocalization(string key)
        {
            var existKey = Data.Common.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetNotificationLocalization(string key)
        {
            var existKey = Data.Notification.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }


        public string GetErrorLocalization(string key)
        {
            var existKey = Data.Error.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetDialogTitleLocalization(string key)
        {
            var existKey = Data.DialogTitle.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetDashboardPageLocalization(string key)
        {
            var existKey = Data.DashboardPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }     

        public string GetProjectPageLocalization(string key)
        {
            var existKey = Data.ProjectPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetTaskPageLocalization(string key)
        {
            var existKey = Data.TaskPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetUserProfilePageLocalization(string key)
        {
            var existKey = Data.UserProfilePage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetBreadCrumbLocalization(string key)
        {
            var existKey = Data.BreadCrumb.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetMainMenuLocalization(string key)
        {
            var existKey = Data.MainMenu.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetAuthLocalization(string key)
        {
            var existKey = Data.Auth.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetErrorPageLocalization(string key)
        {
            var existKey = Data.ErrorPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }
    }
}
