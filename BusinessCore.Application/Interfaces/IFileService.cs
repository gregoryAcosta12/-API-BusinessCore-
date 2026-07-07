using System.IO;
using System.Threading.Tasks;

namespace BusinessCore.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de archivos
    /// Define las operaciones para la gestión de archivos
    /// </summary>
    public interface IFileService
    {
        // Subida de archivos
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string container = "uploads");
        Task<string> UploadFileAsync(byte[] fileBytes, string fileName, string container = "uploads");
        Task<string> UploadImageAsync(Stream imageStream, string fileName, string container = "images");
        Task<string> UploadImageAsync(byte[] imageBytes, string fileName, string container = "images");

        // Descarga de archivos
        Task<byte[]> DownloadFileAsync(string filePath);
        Task<Stream> DownloadFileAsStreamAsync(string filePath);
        Task<string> GetFileUrlAsync(string filePath);

        // Gestión de archivos
        Task<bool> DeleteFileAsync(string filePath);
        Task<bool> FileExistsAsync(string filePath);
        Task<FileInfoDto> GetFileInfoAsync(string filePath);
        Task<string> GenerateUniqueFileNameAsync(string originalFileName);

        // Validaciones
        bool IsValidImageExtension(string fileName);
        bool IsValidFileExtension(string fileName, string[] allowedExtensions);
        bool IsFileSizeValid(long fileSize, long maxSizeInBytes);
        string GetContentType(string fileName);
    }

    public class FileInfoDto
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long SizeInBytes { get; set; }
        public string SizeInKB => $"{SizeInBytes / 1024:F2} KB";
        public string SizeInMB => $"{SizeInBytes / (1024 * 1024):F2} MB";
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}