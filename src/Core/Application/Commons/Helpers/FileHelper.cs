using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

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

        public static string Sanitize(string str, bool forceLowercase = true, bool anal = false)
        {
            string[] strip =  { "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "=", "+", "[", "{", "]",
                   "}", "\\", "|", ";", ":", "\"", "'", "&#8216;", "&#8217;", "&#8220;", "&#8221;", "&#8211;", "&#8212;",
                   "â€", "â€", ",", "<", ".", ">", "/", "?" };
            var clean = Regex.Replace(str, "<[^>]*>", "");
            for (var i = 0; i < strip.Length; i++)
            {
                clean = clean.Replace(strip[i], "");
            }
            clean = clean.Trim();
            clean = clean.Replace("–", "-"); //replace long hyphen by short hyphen
            clean = Regex.Replace(clean, "\\s+", "-");
            clean = anal ? Regex.Replace(clean, "[^a-zA-Z0-9]", "") : clean;
            return forceLowercase ? clean.ToLower() : clean;
        }
    }

    public class FileResponse
    {
        public string FileName { get; set; }
        public bool IsValid { get; set; }
        public FileValidationType ValidationType { get; set; }
    }

    public enum FileValidationType
    {
        Type = 1,
        Size = 2
    }

    public static class FileValidateExtensions
    {
        public static FileResponse ValidateFileUpload(IFormFile file, int maxFileSize, string fileTypeAllowed)
        {
            var response = new FileResponse() { FileName = file.FileName, IsValid = true };
            if (!fileTypeAllowed.Contains(Path.GetExtension(file.FileName).Substring(1)))
            {
                response.ValidationType = FileValidationType.Type;
                response.IsValid = false;
                return response;
            }
            if (file.Length > maxFileSize * 1024 * 1024)
            {
                response.ValidationType = FileValidationType.Size;
                response.IsValid = false;
                return response;
            }
            return response;
        }
    }
}