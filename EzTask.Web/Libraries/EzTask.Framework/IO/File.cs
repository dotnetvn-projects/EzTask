using System.IO;
using System.Threading.Tasks;
using SysIO = System.IO;
namespace EzTask.Framework.IO
{
    public class FileIO
    {
        public static void Create(string filePath, byte[] data)
        {
            if (!SysIO.File.Exists(filePath))
            {
                SysIO.File.WriteAllBytes(filePath, data);
            }
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

        public static string GenerateUniqueName(string folder, string fileName)
        {
            string name = fileName;
            var files = DirectoryIO.ReadFiles(folder,
                Path.GetFileNameWithoutExtension(fileName) + "*" + Path.GetExtension(fileName));

            foreach (var file in files)
            {
                //int i = 0;
                //while (File.Exists(fileName))
                //{
                //    if (i == 0)
                //        fileName = fileName.Replace(extension, "(" + ++i + ")" + extension);
                //    else
                //        fileName = fileName.Replace("(" + i + ")" + extension, "(" + ++i + ")" + extension);
                //}
            }
            return name;
        }
    }
}
