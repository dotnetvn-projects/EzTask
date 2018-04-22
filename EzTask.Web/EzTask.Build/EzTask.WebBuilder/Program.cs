using System;
using System.IO;

namespace EzTask.WebBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WebBuilder is copying modules");
            CopyModules(args);
            Console.WriteLine("WebBuilder has finished copying modules");
            Environment.Exit(0);
        }

        static void CopyModules(string[] args)
        {
            var workFolder = args[0];
            var moduleBuildPath = Path.Combine(workFolder,"EzTask.Web/Modules");
            if (Directory.Exists(moduleBuildPath))
            {
                Directory.Delete(moduleBuildPath, true);
            }

            Directory.CreateDirectory(moduleBuildPath);
            var moduleDevPath = Path.Combine(workFolder,"EzTask.Modules");
            var moduleDevFolder = Directory.GetDirectories(moduleDevPath);

            foreach (var devFolder in moduleDevFolder)
            {
                var moduleConfig = File.ReadAllLines(devFolder + "\\config.txt");
                var moduleName = moduleConfig[0].Split(":")[1].Trim();
                var modulePath = Path.Combine(moduleBuildPath, moduleName);
                Directory.CreateDirectory(modulePath);

                var folderCopy = moduleConfig[1].Split(":")[1].Split(",");

                foreach (var copyPath in folderCopy)
                {
                    var cleanCopyPath = copyPath.Trim();
                    var sourcePath = Path.Combine(devFolder, cleanCopyPath);
                    if (cleanCopyPath.ToLower() != "bin")
                    {
                        var desPath = Path.Combine(modulePath, cleanCopyPath);
                        DirectoryCopy(sourcePath, desPath, true);
                    }
                    else
                    {
                        var files = Directory.GetFiles(sourcePath, moduleName+".dll", SearchOption.AllDirectories);
                        var binPath = Path.Combine(modulePath, "bin");
                        Directory.CreateDirectory(binPath);
                        foreach (var item in files)
                        {
                            var desPath = Path.Combine(binPath, Path.GetFileName(item));
                            File.Copy(item, desPath);                         
                        }
                    }
                }
            }
        }



        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
