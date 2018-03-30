using System;
using System.Collections.Generic;
using System.Text;
using SysIO = System.IO;
namespace EzTask.Framework.IO
{
    public class File
    {
        public static void Create(string filePath)
        {
            if (!SysIO.File.Exists(filePath))
                SysIO.File.Create(filePath);
        }

        public static void AppendText(string text, string filePath)
        {
            if (SysIO.File.Exists(filePath))
            {
                SysIO.File.AppendAllText(filePath, text + "\r\n");
            }                
        }
    }
}
