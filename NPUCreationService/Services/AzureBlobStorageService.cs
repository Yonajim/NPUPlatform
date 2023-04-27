using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace NpuCreationService.Services
{
    /// <summary>
    /// Represents an Azure Blob Storage service, which stores files on Azure Blob Storage.
    /// </summary>
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly CloudBlobContainer _container;

        /// <summary>
        /// Initializes a new instance of the AzureBlobStorageService with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration containing the Azure Blob Storage connection string and container name.</param>
        public AzureBlobStorageService(IConfiguration configuration)
        {
            // Create a CloudStorageAccount from the connection string in the configuration
            var storageAccount = CloudStorageAccount.Parse(configuration["Azure:BlobStorage:ConnectionString"]);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(configuration["Azure:BlobStorage:NpuCreationsContainer"]);
        }

        /// <summary>
        /// Asynchronously saves the specified file to Azure Blob Storage.
        /// </summary>
        /// <param name="image">The file to be saved.</param>
        /// <returns>A task representing the operation, with a string representing the URL of the saved file.</returns>
        public Task<string> SaveFileAsync(IFormFile image)
        {
            // Create a unique file name by combining a GUID with the original file extension
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var blob = _container.GetBlockBlobReference(uniqueFileName);
            blob.Properties.ContentType = image.ContentType;

            // Save the file to Azure Blob Storage
            using var stream = image.OpenReadStream();
            Task.FromResult(blob.UploadFromStreamAsync(stream));

            return Task.FromResult(blob.Uri.AbsoluteUri);
        }

        /// <summary>
        /// Asynchronously deletes the specified file from Azure Blob Storage.
        /// </summary>
        /// <param name="imageUrl">The URL of the file to be deleted.</param>
        /// <returns>A task representing the operation.</returns>
        public Task DeleteFileAsync(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var blobName = Path.GetFileName(uri.LocalPath);
            var blob = _container.GetBlockBlobReference(blobName);

            // Delete the file from Azure Blob Storage
            Task.FromResult(blob.DeleteIfExistsAsync());
            return Task.CompletedTask;
        }
    }
}