using System.IO;

namespace Application.Commons.Helpers
{
    public static class FileHelper
    {
        public static void CreateFolderFromPath(string path)
        {
            if (!Directory.Exists(Path.Combine(path)))
            {
                Directory.CreateDirectory(Path.Combine(path));
            }
        }

        public static void DeleteFolder(string folder)
        {
            Directory.Delete(folder, true);
        }

        public static void CreateFileFromStream(string path, Stream inputStream)
        {
            using (var fileStream = File.Create(path))
            {
                inputStream.CopyTo(fileStream);
                fileStream.Close();
            }
        }

        public static void DeleteFilesInFolder(string rootFolder, string fileName)
        {
            if (File.Exists(Path.Combine(rootFolder, fileName)))
            {
                File.Delete(Path.Combine(rootFolder, fileName));
            }
        }
    }
}