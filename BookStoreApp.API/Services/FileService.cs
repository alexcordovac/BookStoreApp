using BookStoreApp.API.Contracts;
using BookStoreApp.API.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BookStoreApp.API.Services
{
    public class FileService : IFileService
    {
        private readonly StorageOptions _options;

        public FileService(IOptions<StorageOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> SaveFileAsync(string base64, string oldFileName, string saveToPath)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ArgumentException("Invalid file data.");

            var ext = Path.GetExtension(oldFileName);
            var newFileName = $"{Guid.NewGuid()}{ext}";
            var folder = saveToPath;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, newFileName);
            var bytes = Convert.FromBase64String(base64);
            await File.WriteAllBytesAsync(path, bytes);

            // Return only the relative filename or path, not the public URL
            return newFileName;
        }
    }
}
