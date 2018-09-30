using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EzTask.Web.Framework.Infrastructures
{
    public static class WebBuilder
    {
        public static void RunWebBuilder(this IHostingEnvironment env, bool onlyContent = false)
        {
            CopyModules(new string[] { "../../../EzTask.Web/" }, onlyContent);
        }

        private static void CopyModules(string[] args, bool onlyContent)
        {
            string workFolder = args[0];
            string moduleBuildPath = Path.Combine(workFolder, "Presentation/EzTask.Web/Modules");
            if (Directory.Exists(moduleBuildPath))
            {
                try
                {
                    Directory.Delete(moduleBuildPath, true);
                    Directory.CreateDirectory(moduleBuildPath);
                }
                catch { }
            }

            string moduleDevPath = Path.Combine(workFolder, "Modules");
            string[] moduleDevFolder = Directory.GetDirectories(moduleDevPath);

            foreach (string devFolder in moduleDevFolder)
            {
                string[] moduleConfig = File.ReadAllLines(devFolder + "\\config.cf");
                string moduleName = moduleConfig[0].Split(":")[1].Trim();
                string modulePath = Path.Combine(moduleBuildPath, moduleName);
                Directory.CreateDirectory(modulePath);

                string[] folderCopy = moduleConfig[1].Split(":")[1].Split(",");

                foreach (string folder in folderCopy)
                {
                    string copyPath = folder.Trim();
                    string sourcePath = Path.Combine(devFolder, copyPath);
                    if (!Directory.Exists(sourcePath))
                    {
                        continue;
                    }

                    string desPath = string.Empty;

                    if (onlyContent && copyPath.ToLower() == "bin")
                    {
                        continue;
                    }

                    switch (copyPath.ToLower())
                    {
                        case "bin":
                            string[] files = Directory.GetFiles(sourcePath, moduleName + ".dll", SearchOption.AllDirectories);
                            string binPath = Path.Combine(modulePath, "bin");
                            Directory.CreateDirectory(binPath);

                            foreach (string item in files)
                            {
                                desPath = Path.Combine(binPath, Path.GetFileName(item));
                                File.Copy(item, desPath, true);
                            }
                            break;

                        case "wwwroot":
                            desPath = Path.Combine(workFolder, "Presentation/EzTask.Web/wwwroot");
                            DirectoryCopy(sourcePath, desPath, true);
                            break;

                        default:
                            desPath = Path.Combine(modulePath, copyPath);
                            DirectoryCopy(sourcePath, desPath, true);
                            break;
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
                file.CopyTo(temppath, true);
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
