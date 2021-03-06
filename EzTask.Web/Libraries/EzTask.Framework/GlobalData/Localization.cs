﻿using System.Collections.Generic;

namespace EzTask.Framework.GlobalData
{
    public class Localization
    {
        public string Pattern { get; set; }

        public List<Language> Common { get; set; }
        public List<Language> DialogTitle { get; set; }
        public List<Language> Error { get; set; }
        public List<Language> ErrorPage { get; set; }
        public List<Language> MainMenu { get; set; }
        public List<Language> BreadCrumb { get; set; }
        public List<Language> AuthenticationPage { get; set; }
        public List<Language> Notification { get; set; }
        public List<Language> DashboardPage { get; set; }
        public List<Language> ProjectPage { get; set; }
        public List<Language> TaskPage { get; set; }
        public List<Language> UserProfilePage { get; set; }
    }
}
