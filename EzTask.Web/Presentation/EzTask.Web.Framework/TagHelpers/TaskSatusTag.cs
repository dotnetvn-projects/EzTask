using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using EzTask.Web.Framework.Data;
using EzTask.Models.Enum;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using EzTask.Framework.Common;

namespace EzTask.Web.Framework.TagHelpers
{
    [HtmlTargetElement("task-status")]
    public class TaskSatusTag: TagHelper
    {
        private StaticResources _resources;
        public TaskSatusTag(StaticResources resources)
        {
            _resources = resources;
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression Data { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }
         

            var dataValue = Data.ModelExplorer.GetSimpleDisplayText();
            var status = dataValue.ToEnum<TaskItemStatus>();

            var htmlString = _resources.GetTaskStatusUIElement(status);

            output.Content.SetHtmlContent(htmlString);
        }
    }
}
