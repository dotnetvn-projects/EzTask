using EzTask.Framework.Common;
using EzTask.Framework.ModelValidatorAttributes;
using EzTask.Model.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace EzTask.Modules.Dashboard.ViewModels
{
    public class ToDoItemViewModel
    {
        public int Id { get; set; }

        public string ManagedCode { get; set; }

        [RequiredField(errorLanguageKey: "TodoItemTitleRequired", 
            languagePageSetting: "DashboardPage")]
        public string Title { get; set; }

        public int AccountId { get; set; }

        [RequiredField(errorLanguageKey: "TodoItemCompleteOnRequied",
            languagePageSetting: "DashboardPage")]
        public string CompleteOn { get; set; }

        public short Priority { get; set; }

        public short Status { get; set; }

        public List<SelectListItem> PriorityList { get; set; }
        public List<SelectListItem> StatusList { get; set; }

        public ToDoItemViewModel()
        {
            CompleteOn = DateTime.Now.AddDays(1).ToDateString();
            Status = ToDoItemStatus.Waiting.ToInt16<ToDoItemStatus>();
            Priority = ToDoItemPriority.Medium.ToInt16<ToDoItemPriority>();
            ManagedCode = Guid.NewGuid().ToString();
        }
    }
}
