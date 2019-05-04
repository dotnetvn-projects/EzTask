using EzTask.Framework.Common;
using EzTask.Framework.Data;
using System.Linq;

namespace EzTask.Framework.Infrastructures
{
    public class LocalizationMapper
    {
        public Localization Data { get; set; }

        public LocalizationMapper(string filepath)
        {
            Data = XmlUtilities.Deserialize<Localization>(filepath);
        }

        /// <summary>
        /// Get common string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetCommonLocalization(string key)
        {
            var existKey = Data.Common.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get notificaiton string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetNotificationLocalization(string key)
        {
            var existKey = Data.Notification.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get error string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetErrorLocalization(string key)
        {
            var existKey = Data.Error.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get dialog title string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDialogTitleLocalization(string key)
        {
            var existKey = Data.DialogTitle.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get dashboard string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDashboardPageLocalization(string key)
        {
            var existKey = Data.DashboardPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get project string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetProjectPageLocalization(string key)
        {
            var existKey = Data.ProjectPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get task string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetTaskPageLocalization(string key)
        {
            var existKey = Data.TaskPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get user profile string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetUserProfilePageLocalization(string key)
        {
            var existKey = Data.UserProfilePage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get breadcrumb string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetBreadCrumbLocalization(string key)
        {
            var existKey = Data.BreadCrumb.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get main menu string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetMainMenuLocalization(string key)
        {
            var existKey = Data.MainMenu.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get auth string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAuthLocalization(string key)
        {
            var existKey = Data.AuthenticationPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get error page string resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
