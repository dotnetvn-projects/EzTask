using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Web.Framework.TagHelpers
{
    [HtmlTargetElement("task-status")]
    public class TaskSatus: TagHelper
    {
        [HtmlAttributeName("status")]
        public TaskSatus Status { get; set; }

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
            tag.InnerHtml.AppendHtml(string.Format("", ""));

            output.Content.SetHtmlContent(tag);
        }
    }
}
