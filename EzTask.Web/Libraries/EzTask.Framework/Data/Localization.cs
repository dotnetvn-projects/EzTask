using EzTask.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzTask.Framework.Data
{
    public class Localization
    {
        public string Pattern { get; set; }

        public List<Language> CommonMessage { get; set; }
        public List<Language> MessageTitle { get; set; }
        public List<Language> ErrorMessage { get; set; }
        public List<Language> SuccessMessage { get; set; }  

        public List<Language> DashboardPage { get; set; }

        public List<Language> ProjectPage { get; set; }

    }
}
