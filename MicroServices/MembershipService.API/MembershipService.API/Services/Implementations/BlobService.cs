namespace MembershipService.API.Services.Implementations
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using MembershipService.API.Services.Interfaces;

    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _container;

        // Constructor mặc định không có tham số
        public BlobService()
        {
            // Lấy connection string và container name từ biến môi trường (được nạp từ .env)
            var connectionString = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_CONNECTION_STRING");
            var containerName = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_CONTAINER_NAME");

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
            {
                throw new InvalidOperationException("Azure Blob Storage configuration is missing from environment variables.");
            }

            // Khởi tạo BlobContainerClient
            _container = new BlobContainerClient(connectionString, containerName);
            _container.CreateIfNotExists(PublicAccessType.Blob); // Cho phép đọc ảnh qua URL
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
