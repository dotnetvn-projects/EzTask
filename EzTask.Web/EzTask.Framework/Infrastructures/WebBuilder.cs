using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace EzTask.Framework.Infrastructures
{
    public static class WebBuilder
    {
        public static void Run(IHostingEnvironment env)
        {
            //var executeFile = Path.Combine(env.ContentRootPath, "build.bat");
            //if (File.Exists(executeFile))
            //{
            //    Process p = new Process();
            //    p.StartInfo.FileName = "build.bat";
            //    p.StartInfo.WorkingDirectory = Path.GetDirectoryName(env.ContentRootPath);
            //    p.StartInfo.UseShellExecute = false;

            //    // Run the process and wait for it to complete
            //    p.Start();
            //    p.WaitForExit(3000);
            //}
        }
    }
}
