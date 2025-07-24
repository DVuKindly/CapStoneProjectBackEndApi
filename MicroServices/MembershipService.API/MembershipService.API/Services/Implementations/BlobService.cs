namespace MembershipService.API.Services.Implementations
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using MembershipService.API.Services.Interfaces;

    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _container;

        public BlobService(IConfiguration config)
        {
            var connectionString = config["AzureBlobStorage:ConnectionString"];
            var containerName = config["AzureBlobStorage:ContainerName"];

            _container = new BlobContainerClient(connectionString, containerName);
            _container.CreateIfNotExists(PublicAccessType.Blob); // 👈 Cho phép đọc ảnh qua URL
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var blobClient = _container.GetBlobClient(file.FileName);

            var headers = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };

            await using var stream = file.OpenReadStream();

            // Overwrite bằng cách gọi UploadAsync(stream, overwrite: true)
            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = headers
            }, cancellationToken: default);

            return blobClient.Uri.AbsoluteUri;
        }

    }
}