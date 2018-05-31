using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

        public static async Task<byte[]> ReadFile(string path)
        {
            return await System.IO.File.ReadAllBytesAsync(path);
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
