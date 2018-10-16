using EzTask.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzTask.Framework.Infrastructures
{
    public class Localization
    {
        public string Pattern { get; set; }
        public List<Language> HomePage { get; set; }
        public List<Language> ProjectPage { get; set; }


        public string GetHomePageLang(string key)
        {
            var existKey = HomePage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }

        public string GetProjectPageLang(string key)
        {
            var existKey = ProjectPage.FirstOrDefault(c => c.Key == key);
            if (existKey != null)
            {
                return existKey.Content;
            }
            return string.Empty;
        }
    }
}
