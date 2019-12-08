using EzTask.Framework.GlobalData;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.Controllers
{
    public class TaskMediaController : BaseController
    {
        public TaskMediaController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        /// <summary>
        /// Download attachment file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("task/attachment/{id}/download.html")]
        public async Task<ActionResult> DowloadAttachment(int id)
        {
            var attachment = await EzTask.Task.GetAttachment(id);
            if (attachment == null)
            {
                return Content(Context.GetStringResource("InvalidRequest", StringResourceType.Error));
            }

            if (attachment.FileType.ToLower().Contains("image") || attachment.FileType.ToLower().Contains("pdf"))
            {
                return File(attachment.FileData, attachment.FileType);
            }
            return File(attachment.FileData, attachment.FileType, attachment.FileName);
        }

        /// <summary>
        /// remove attachment file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("task/attachment/delete.html")]
        public async Task<ActionResult> RemoveAttachment(int id)
        {
            await EzTask.Task.DeleteAttachment(id);
            return Ok();
        }
    }
}
