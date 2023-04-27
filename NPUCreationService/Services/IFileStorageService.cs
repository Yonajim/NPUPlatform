namespace NpuCreationService.Services
{
    /// <summary>
    /// Represents the interface for file storage services.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Asynchronously saves the specified file to the storage system.
        /// </summary>
        /// <param name="image">The file to be saved.</param>
        /// <returns>A task representing the operation, with a string representing the URL of the saved file.</returns>
        Task<string> SaveFileAsync(IFormFile image);

        /// <summary>
        /// Asynchronously deletes the specified file from the storage system.
        /// </summary>
        /// <param name="imageUrl">The URL of the file to be deleted.</param>
        /// <returns>A task representing the operation.</returns>
        public Task DeleteFileAsync(string imageUrl);
    }
}