using System;
using System.Collections.Generic;
using System.Linq;
using SysIO = System.IO;

namespace EzTask.Framework.IO
{
    public class DirectoryIO
    {
        public static void CreateDir(string path)
        {
            if (!SysIO.Directory.Exists(path))
                SysIO.Directory.CreateDirectory(path);
        }

        public static void DeleteDir(string path)
        {
            if (SysIO.Directory.Exists(path))
                SysIO.Directory.Delete(path, true);
        }

        public static List<string> ReadFiles(string path, string searchPattern = "")
        {
            if (SysIO.Directory.Exists(path))
            {
                var files = new List<string>();
                if (string.IsNullOrEmpty(searchPattern))
                {
                    files = SysIO.Directory.GetFiles(path).ToList();
                }
                else
                {
                    files = SysIO.Directory.GetFiles(path, searchPattern).ToList();
                }
                return files;
            }
            return new List<string>();
        }
    }
}
