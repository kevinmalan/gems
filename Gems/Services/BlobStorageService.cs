using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using static Gems.Services.FileService;

namespace Gems.Services
{
    public class BlobStorageService
    {
        private static readonly string _storageAccountConnectionString = Configuration["Blob:StorageAccountConnectionString"];

        public static async Task<BlobContainerClient> GetOrCreateContainerAsync(string containerName)
        {
            var blobServiceClient = new BlobServiceClient(_storageAccountConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            return containerClient;
        }

        public static async Task UploadOrReplaceBlobAsync(BlobContainerClient containerClient, string resourceName)
        {
            var resourcePath = @$"{Configuration["Blob:ResourcesPath"]}{resourceName}";
            var blobClient = containerClient.GetBlobClient(resourceName);

            using FileStream fs = File.OpenRead(resourcePath);
            await blobClient.UploadAsync(fs, true);
        }

        public static async Task DownloadBlobAsync(BlobContainerClient containerClient, string resourceName)
        {
            var blobClient = containerClient.GetBlobClient(resourceName);

            if (!await blobClient.ExistsAsync())
                throw new Exception($"Resource {resourceName} does not exist!");

            var folderPath = Configuration["Blob:DownloadedPath"];
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, resourceName);

            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
            using FileStream fs = File.OpenWrite(filePath);
            await blobDownloadInfo.Content.CopyToAsync(fs);
            fs.Close();
        }
    }
}