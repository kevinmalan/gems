using Gems.Services;
using static Gems.Services.FileService;

namespace Gems.Flows
{
    public class BlobFlow
    {
        /// <summary>
        /// Reads Blob configuration from appsettings.json
        /// And then creates a container if one does not already exist
        /// And uploads a resource to the container and downloads it afterwards
        /// </summary>
        /// <returns></returns>
        public async Task Invoke()
        {
            var resourcePath = Configuration["Blob:ResourcesPath"];
            var downloadedPath = Configuration["Blob:DownloadedPath"];
            Console.WriteLine($"Copy a resource (such as a image / document / etc) to {resourcePath}");
            Console.WriteLine("Type the file's name and extension into the console:");
            var resourceName = Console.ReadLine();
            var container = Configuration["Blob:Container"];
            var containerClient = await BlobStorageService.GetOrCreateContainerAsync(container);
            await BlobStorageService.UploadOrReplaceBlobAsync(containerClient, resourceName);
            await BlobStorageService.DownloadBlobAsync(containerClient, resourceName);

            Console.WriteLine($"{resourceName} has been uploaded to Blob Storage container '{container}' and downloaded to '{downloadedPath}'");
        }
    }
}