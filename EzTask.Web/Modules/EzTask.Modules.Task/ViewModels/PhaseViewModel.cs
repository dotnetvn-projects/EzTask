using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EzTask.Modules.Task.ViewModels
{
    public class PhaseViewModel
    {
        public int PhaseId { get; set; }
        public int ProjectId { get; set; }
        public string PhaseName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public short Status { get; set; }
        public bool IsDefault { get; set; }
        public List<SelectListItem> StatusList { get; set; }

        public PhaseViewModel()
        {
            StatusList = new List<SelectListItem>();
        }
    }
}
