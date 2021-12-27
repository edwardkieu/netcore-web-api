using Microsoft.AspNetCore.Http;
using System.IO;

namespace Application.Commons.Extensions
{
    public class FileValidationResponse
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

    public static class FormFileExtensions
    {
        public static FileValidationResponse ValidateFileUpload(this IFormFile formFile, int maxFileSize, string fileTypeAllowed)
        {
            var response = new FileValidationResponse
            {
                FileName = formFile.FileName,
                IsValid = true
            };

            if (!fileTypeAllowed.Contains(Path.GetExtension(formFile.FileName)))
            {
                response.ValidationType = FileValidationType.Type;
                response.IsValid = false;
                return response;
            }

            //TenMegaBytes = 10 * 1024 * 1024;
            if (formFile.Length > maxFileSize * 1024 * 1024)
            {
                response.ValidationType = FileValidationType.Size;
                response.IsValid = false;
                return response;
            }

            return response;
        }
    }
}