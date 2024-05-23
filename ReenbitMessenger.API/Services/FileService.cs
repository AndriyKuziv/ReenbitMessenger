using Azure.Storage;
using Azure.Storage.Blobs;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.API.Services
{
    public class FileService
    {
        private readonly string _storageAccount = "rbmessengerstorage";
        private readonly string _key = "";
        private readonly BlobContainerClient _containerClient;

        public FileService()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobUrl = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUrl), credential);
            _containerClient = blobServiceClient.GetBlobContainerClient("users-avatars");
        }

        public async Task<BlobResponse> GetUserAvatar()
        {
            

            return null;
        }

        public async Task<string> UploadUserAvatar(IFormFile blob)
        {
            BlobClient client = _containerClient.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            return client.Uri.AbsoluteUri;
        }
    }
}
