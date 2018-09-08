using EzTask.Framework.Common;
using EzTask.Models;
using EzTask.Models.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzTask.Web.Framework.Data
{
    public class StaticResources
    {
        private Dictionary<TaskItemStatus, string> _taskStatusUIElement;

        public StaticResources ()
        {
            Init();
        }

        private void Init()
        {
            _taskStatusUIElement = new Dictionary<TaskItemStatus, string>()
            {
                { TaskItemStatus.Assiged, "<i class=\"fa fa-anchor\"></i> <span class=\"space-left\">Assiged</span>" },
                { TaskItemStatus.Closed, "<i class=\"fa fa-check-circle text-success\"></i><span class=\"space-left\">Closed</span>" },
                { TaskItemStatus.Failed, "<i class=\"fa fa-close text-red\"></i> Failed" },
                { TaskItemStatus.Feedback, "<i class=\"fa fa-coffee text-fuchsia\"></i><span class=\"space-left\">Feedback</span>" },
                { TaskItemStatus.Open, "<i class=\"fa fa-circle-o text-yellow\"></i><span class=\"space-left\">Open</span>" },
                { TaskItemStatus.Resovled, "<i class=\"fa fa-thumbs-o-up\"></i><span class=\"space-left\">Resovled</span>" },
                { TaskItemStatus.Working, "<i class=\"fa fa-plane text-yellow\"></i><span class=\"space-left\">Working</span>" }
            };
        }            

        public string GetTaskStatusUIElement(TaskItemStatus taskStatus)
        {
            return _taskStatusUIElement[taskStatus];
        }

        /// <summary>
        /// Build Priority SelectList items
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> BuildPrioritySelectList(Int16 selectedId = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var dataItems = EnumUtilities.ToList<TaskPriority>();
            if (dataItems.Any())
            {
                foreach (var data in dataItems)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = data,
                        Value = ((Int16)data.ToEnum<TaskPriority>()).ToString()
                    };

                    if (selectedId != 0)
                    {
                        var enumData = selectedId.ToEnum<TaskPriority>();
                        if (data == enumData.ToString())
                        {
                            selectItem.Selected = true;
                        }
                    }
                    list.Add(selectItem);
                }
            }
            if (selectedId == 0)
            {
                list[0].Selected = true;
            }
            return list;
        }

        /// <summary>
        /// Build Phrase SelectList items
        /// </summary>
        /// <param name="phrases"></param>
        /// <returns></returns>
        public static List<SelectListItem> BuildPhraseSelectList(IEnumerable<PhraseModel> phrases,
            int selectedId = 0)
        {
            List<SelectListItem> phraseItems = new List<SelectListItem>();

            if (phrases.Any())
            {
                foreach (var phr in phrases)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = phr.PhraseName,
                        Value = phr.Id.ToString()
                    };
                    if (phr.Id == selectedId)
                    {
                        selectItem.Selected = true;
                    }
                    phraseItems.Add(selectItem);
                }
            }
            if (selectedId == 0)
            {
                phraseItems[0].Selected = true;
            }
            return phraseItems;
        }

        /// <summary>
        /// Build Assignees SelectList items
        /// </summary>
        /// <param name="assignees"></param>
        /// <returns></returns>
        public static List<SelectListItem> BuildAssigneeSelectList(IEnumerable<AccountModel> assignees,
            int selectedId = 0)
        {
            List<SelectListItem> assigneeItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "0",
                    Text = "Non-Assigned"
                }
            };

            if (assignees.Any())
            {
                foreach (var assign in assignees)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = assign.DisplayName,
                        Value = assign.AccountId.ToString()
                    };
                    if (assign.AccountId == selectedId)
                    {
                        selectItem.Selected = true;
                    }
                    assigneeItems.Add(selectItem);
                }
            }
            if (selectedId == 0)
            {
                assigneeItems[0].Selected = true;
            }
            return assigneeItems;
        }

        /// <summary>
        /// Build Status SelectList items
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> BuildStatusSelectList(Int16 selectedId = 0)
        {
            List<SelectListItem> statusItems = new List<SelectListItem>();
            var statuses = EnumUtilities.ToList<TaskItemStatus>();
            if (statuses.Any())
            {
                foreach (var status in statuses)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = status,
                        Value = ((Int16)status.ToEnum<TaskItemStatus>()).ToString()
                    };

                    if (selectedId != 0)
                    {
                        var statusEnum = selectedId.ToEnum<TaskItemStatus>();
                        if (status == statusEnum.ToString())
                        {
                            selectItem.Selected = true;
                        }
                    }
                    statusItems.Add(selectItem);
                }
            }
            if (selectedId == 0)
            {
                statusItems[0].Selected = true;
            }
            return statusItems;
        }
    }
}
