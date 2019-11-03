using EzTask.Framework.Common;
using EzTask.Framework.GlobalData;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

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
                { TaskItemStatus.Assiged, string.Format("<i class=\"fa fa-anchor\"></i> <span class=\"space-left\">{0}</span>",
                                                            Context.GetStringResource("Assiged", StringResourceType.TaskPage)) },

                { TaskItemStatus.Closed, string.Format("<i class=\"fa fa-check-circle text-success\"></i><span class=\"space-left\">{0}</span>",
                                                            Context.GetStringResource("Closed", StringResourceType.TaskPage))},

                { TaskItemStatus.Failed, string.Format("<i class=\"fa fa-close text-red\"></i><span class=\"space-left\">{0}</span>",
                                                            Context.GetStringResource("Failed", StringResourceType.TaskPage))},

                { TaskItemStatus.Feedback, string.Format("<i class=\"fa fa-coffee text-fuchsia\"></i><span class=\"space-left\">{0}</span>",
                                                            Context.GetStringResource("Feedback", StringResourceType.TaskPage))},

                { TaskItemStatus.Open, string.Format("<i class=\"fa fa-circle-o text-yellow\"></i><span class=\"space-left\">{0}</span>",
                                                            Context.GetStringResource("Open", StringResourceType.TaskPage))},

                { TaskItemStatus.Resovled, string.Format("<i class=\"fa fa-thumbs-o-up\"></i><span class=\"space-left\">{0}</span>",
                                                            Context.GetStringResource("Resovled", StringResourceType.TaskPage))},

                { TaskItemStatus.Working, string.Format("<i class=\"fa fa-plane text-yellow\"></i><span class=\"space-left\">{0}</span>",
                                                            Context.GetStringResource("Working", StringResourceType.TaskPage))}
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
                        Text = Context.GetStringResource(data + "Priority", StringResourceType.TaskPage),
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
        public static List<SelectListItem> BuildPhaseSelectList(IList<PhaseModel> phases,
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
        public static List<SelectListItem> BuildAssigneeSelectList(IList<ProjectMemberModel> assignees,
            int selectedId = 0)
        {
            List<SelectListItem> assigneeItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "0",
                    Text = Context.GetStringResource("NonAssigned", StringResourceType.TaskPage)
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

        /// <summary>
        /// Build Todo item status SelectList items
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> BuildToDoItemStatusSelectList(short selectedId = 0)
        {
            List<SelectListItem> statusItems = new List<SelectListItem>();
            var statuses = EnumUtilities.ToList<ToDoItemStatus>();
            if (statuses.Any())
            {
                foreach (var status in statuses)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = status,
                        Value = ((short)status.ToEnum<ToDoItemStatus>()).ToString()
                    };

                    if (selectedId != 0)
                    {
                        var statusEnum = selectedId.ToEnum<ToDoItemStatus>();
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
        /// Build Todo item priority SelectList items
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> BuildToDoItemPrioritySelectList(short selectedId = 0)
        {
            List<SelectListItem> priorityItems = new List<SelectListItem>();
            var priorities = EnumUtilities.ToList<ToDoItemPriority>();
            if (priorities.Any())
            {
                foreach (var priority in priorities)
                {
                    var selectItem = new SelectListItem
                    {
                        Text = priority,
                        Value = ((short)priority.ToEnum<ToDoItemPriority>()).ToString()
                    };

                    if (selectedId != 0)
                    {
                        var priorityEnum = selectedId.ToEnum<ToDoItemPriority>();
                        if (priority == priorityEnum.ToString())
                        {
                            selectItem.Selected = true;
                        }
                    }
                    priorityItems.Add(selectItem);
                }
            }
            if (selectedId == 0)
            {
                priorityItems[0].Selected = true;
            }
            return priorityItems;
        }
    }
}
