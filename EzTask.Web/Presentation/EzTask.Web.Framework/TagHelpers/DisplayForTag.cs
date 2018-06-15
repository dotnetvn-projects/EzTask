using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace EzTask.Web.Framework.TagHelpers
{
    [HtmlTargetElement("display-for")]
    public class DisplayForTag : TagHelper
    {
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

            var type = Data.ModelExplorer.ModelType;
            string text = Data.ModelExplorer.GetSimpleDisplayText();

            if (type == typeof(DateTime?) || type == typeof(DateTime))
            {
                DateTime.TryParse(text, out DateTime date);
                if (date == null || date == DateTime.MinValue)
                {
                    text = string.Empty;
                }
                else
                {
                    text = date.ToString("MM/dd/yyyy");
                }            
            }

            var tag = new TagBuilder("span");
            tag.InnerHtml.AppendHtml(string.Format("<span>{0}</span>", text));

            output.Content.SetHtmlContent(tag);
        }
    }
}
