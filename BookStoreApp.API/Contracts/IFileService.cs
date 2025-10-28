namespace BookStoreApp.API.Contracts
{
    public interface IFileService
    {
        /// <summary>
        /// Save a base64 string as a file and return the file path
        /// </summary>
        /// <param name="base64">Enconded file base64</param>
        /// <param name="oldFileName">Name of the file to save, so we can get the extension of the file.</param>m>
        /// <param name="saveToPath">Path where the file will be stored.</param>
        /// <returns></returns>
        Task<string> SaveFileAsync(string base64, string oldFileName, string saveToPath);
    }
}
