namespace NpuCreationService.Services
{
    /// <summary>
    /// Represents a local file storage service, which stores files on the local filesystem.
    /// </summary>
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _storagePath;

        /// <summary>
        /// Initializes a new instance of the LocalFileStorageService with the specified storage path.
        /// </summary>
        /// <param name="storagePath">The path to the storage directory.</param>
        public LocalFileStorageService(string storagePath)
        {
            _storagePath = storagePath;
        }

        /// <summary>
        /// Asynchronously saves the specified file to the local filesystem.
        /// </summary>
        /// <param name="image">The file to be saved.</param>
        /// <returns>A task representing the operation, with a string representing the file path of the saved file.</returns>
        public async Task<string> SaveFileAsync(IFormFile image)
        {
            try
            {
                var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                string filePath = Path.Combine(_storagePath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while saving the file: {ex.Message}");
            }
        }

        /// <summary>
        /// Asynchronously deletes the specified file from the local filesystem.
        /// </summary>
        /// <param name="filePath">The file path of the file to be deleted.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task DeleteFileAsync(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deleting the file: {ex.Message}");
            }

            await Task.CompletedTask;
        }
    }
}