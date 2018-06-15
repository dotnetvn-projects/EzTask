using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using EzTask.Web.Framework.Data;
using EzTask.Models.Enum;

namespace EzTask.Web.Framework.TagHelpers
{
    [HtmlTargetElement("task-status")]
    public class TaskSatusTag: TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public TaskStatus Status { get; set; }

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
         
            var tag = new TagBuilder("span");
         //   var htmlString = StaticResources.TaskStatusUIElement[Status];
            tag.InnerHtml.AppendHtml("vvvv");

            output.Content.SetHtmlContent(tag);
        }
    }
}
