using System;
using System.Collections.Generic;
using System.Linq;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Task.Models;
using EzTask.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EzTask.Framework.Common;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class TaskItemController : CoreController
    {
        public TaskItemController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        [Route("taskitem/generate-new-item.html")]
        [HttpPost]
        public async Task<IActionResult> PrepairNewItem(int projectId)
        {
            var newTask = new TaskItemViewModel();

            var phrases = await EzTask.Phrase.GetPhrase(projectId);
            newTask.PhraseList = BuildPhraseSelectList(phrases);

            var assignees = await EzTask.Project.GetAccountList(projectId);
            newTask.AssigneeList = BuildAssigneeSelectList(assignees);

            newTask.StatusList = BuildStatusSelectList();

            return PartialView("_CreateOrUpdateTask", newTask);
        }

        #region Non-Action
        /// <summary>
        /// Build Phrase SelectList items
        /// </summary>
        /// <param name="phrases"></param>
        /// <returns></returns>
        private static List<SelectListItem> BuildPhraseSelectList(IEnumerable<PhraseModel> phrases,
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
        private static List<SelectListItem> BuildAssigneeSelectList(IEnumerable<AccountModel> assignees,
            int selectedId = 0)
        {
            List<SelectListItem> assigneeItems = new List<SelectListItem>();
            assigneeItems.Add(new SelectListItem
            {
                Value = "0",
                Text = "Non-Assigned"
            });

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
        private static List<SelectListItem> BuildStatusSelectList(Int16 selectedId = 0)
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
                        Value = ((int)status.ToEnum<TaskItemStatus>()).ToString()
                    };

                    if (selectedId != 0)
                    {
                        var statusEnum = selectedId.ToEnum<TaskItemStatus>();
                        if(status == statusEnum.ToString())
                        {
                            selectItem.Selected = true;
                        }
                    } 
                    statusItems.Add(selectItem);
                }
            }
            if(selectedId == 0)
            {
                statusItems[0].Selected = true;
            }
            return statusItems;
        }
        #endregion
    }
}