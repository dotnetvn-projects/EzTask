﻿using EzTask.Framework.Common;
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
        /// Build Phase SelectList items
        /// </summary>
        /// <param name="phases"></param>
        /// <returns></returns>
        public static List<SelectListItem> BuildPhaseSelectList(IEnumerable<PhaseModel> phases,
            int selectedId = 0)
        {
            List<SelectListItem> phaseItems = new List<SelectListItem>();

            if (phases.Any())
            {
                foreach (var phr in phases)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = phr.PhaseName,
                        Value = phr.Id.ToString()
                    };
                    if (phr.Id == selectedId)
                    {
                        selectItem.Selected = true;
                    }
                    phaseItems.Add(selectItem);
                }
            }
            if (selectedId == 0)
            {
                phaseItems[0].Selected = true;
            }
            return phaseItems;
        }

        /// <summary>
        /// Build Assignees SelectList items
        /// </summary>
        /// <param name="assignees"></param>
        /// <returns></returns>
        public static List<SelectListItem> BuildAssigneeSelectList(IEnumerable<ProjectMemberModel> assignees,
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
        public static List<SelectListItem> BuildTaskStatusSelectList(Int16 selectedId = 0)
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

        /// <summary>
        /// Build Status SelectList items
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> BuildPhaseStatusSelectList(Int16 selectedId = 0)
        {
            List<SelectListItem> statusItems = new List<SelectListItem>();
            var statuses = EnumUtilities.ToList<PhaseStatus>();
            if (statuses.Any())
            {
                foreach (var status in statuses)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = status,
                        Value = ((Int16)status.ToEnum<PhaseStatus>()).ToString()
                    };

                    if (selectedId != 0)
                    {
                        var statusEnum = selectedId.ToEnum<PhaseStatus>();
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
