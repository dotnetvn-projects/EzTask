using System.Collections.Generic;

namespace EzTask.Model
{
    public class PageListModel
    {
        public List<int> Pages { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public string Css { get; set; }

        public PageListModel()
        {
            Css = string.Empty;
            Pages = new List<int>();
        }
    }
}
