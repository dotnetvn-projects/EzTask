using System;
using System.Collections.Generic;
using SysIO = System.IO;

namespace EzTask.Framework.IO
{
    public class Directory
    {
        public static void CreateDir(string path)
        {
            if(!SysIO.Directory.Exists(path))
                SysIO.Directory.CreateDirectory(path);
        }

        public static void DeleteDir(string path)
        {
            if (SysIO.Directory.Exists(path))
                SysIO.Directory.Delete(path, true);
        }
    }
}
