using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.Models
{
    public class PhraseViewModel
    {
        public int PhraseId { get; set; }
        public int ProjectId { get; set; }
        public string PhraseName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public short Status { get; set; }
        public bool IsDefault { get; set; }
        public List<SelectListItem> StatusList { get; set; }

        public PhraseViewModel()
        {
            StatusList = new List<SelectListItem>();
        }
    }
}
