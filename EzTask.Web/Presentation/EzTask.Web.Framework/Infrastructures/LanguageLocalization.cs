using EzTask.Framework.Common;
using EzTask.Framework.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Interface;
using EzTask.Web.Framework.WebContext;

namespace EzTask.Web.Framework.Infrastructures
{
    public class LanguageLocalization : ILanguageLocalization
    {
        private static IWebHostEnvironment _webHostEnvironment;
        private static SessionManager _sessionManager;


        public LanguageLocalization(IWebHostEnvironment webHostEnvironment, SessionManager sessionManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// Get by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public string GetLocalization(string key, object resourceType)
        {
            var lang = GetLanguageLocalization();
            switch (resourceType)
            {
                case StringResourceType.Common:
                    return lang.GetCommonLocalization(key);
                case StringResourceType.DashboardPage:
                    return lang.GetDashboardPageLocalization(key);
                case StringResourceType.Error:
                    return lang.GetErrorLocalization(key);
                case StringResourceType.DialogTitle:
                    return lang.GetDialogTitleLocalization(key);
                case StringResourceType.ProjectPage:
                    return lang.GetProjectPageLocalization(key);
                case StringResourceType.TaskPage:
                    return lang.GetTaskPageLocalization(key);
                case StringResourceType.UserProfilePage:
                    return lang.GetUserProfilePageLocalization(key);
                case StringResourceType.BreadCrumb:
                    return lang.GetBreadCrumbLocalization(key);
                case StringResourceType.MainMenu:
                    return lang.GetMainMenuLocalization(key);
                case StringResourceType.Notification:
                    return lang.GetNotificationLocalization(key);
                case StringResourceType.AuthenticationPage:
                    return lang.GetAuthLocalization(key);
                case StringResourceType.ErrorPage:
                    return lang.GetErrorPageLocalization(key);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public string GetLocalization(string key, string resourceType)
        {
            var lang = GetLanguageLocalization();
            var type = resourceType.ToEnum<StringResourceType>();
            switch (type)
            {
                case StringResourceType.Common:
                    return lang.GetCommonLocalization(key);
                case StringResourceType.DashboardPage:
                    return lang.GetDashboardPageLocalization(key);
                case StringResourceType.Error:
                    return lang.GetErrorLocalization(key);
                case StringResourceType.DialogTitle:
                    return lang.GetDialogTitleLocalization(key);
                case StringResourceType.ProjectPage:
                    return lang.GetProjectPageLocalization(key);
                case StringResourceType.TaskPage:
                    return lang.GetTaskPageLocalization(key);
                case StringResourceType.UserProfilePage:
                    return lang.GetUserProfilePageLocalization(key);
                case StringResourceType.BreadCrumb:
                    return lang.GetBreadCrumbLocalization(key);
                case StringResourceType.MainMenu:
                    return lang.GetMainMenuLocalization(key);
                case StringResourceType.Notification:
                    return lang.GetNotificationLocalization(key);
                case StringResourceType.AuthenticationPage:
                    return lang.GetAuthLocalization(key);
                case StringResourceType.ErrorPage:
                    return lang.GetErrorPageLocalization(key);
            }

            return string.Empty;
        }

        /// <summary>
        /// Set language
        /// </summary>
        /// <param name="languageSetting"></param>
        public void SetLocalization(string languageSetting)
        {
            if (string.IsNullOrEmpty(languageSetting))
            {
                languageSetting = "lang-en.xml";
            }

            var langPath = _webHostEnvironment.GetRootContentUrl() + "/resources/languages/" + languageSetting;

            var mapper = new LocalizationMapper(langPath);

            _sessionManager.SetObject(SessionKey.LanguageSetting, mapper);
        }

        /// <summary>
        /// Get language
        /// </summary>
        /// <returns></returns>
        private static LocalizationMapper GetLanguageLocalization()
        {
            var setting = _sessionManager.GetObject<LocalizationMapper>(SessionKey.LanguageSetting);
            return setting;
        }

        /// <summary>
        /// Language setting is available or not
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                return GetLanguageLocalization() != null;
            }
        }
    }
}
