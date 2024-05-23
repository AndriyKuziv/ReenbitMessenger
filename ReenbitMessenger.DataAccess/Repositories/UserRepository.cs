using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<IdentityUser, string>, IUserRepository
    {
        private const string containerName = "users-avatars";
        private readonly BlobContainerClient _containerClient;

        public UserRepository(MessengerDataContext dbContext,
            BlobServiceClient blobServiceClient) : base(dbContext)
        {
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            return _dbContext.Users.AsEnumerable();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async new Task<IdentityUser> UpdateAsync(string id, IdentityUser entity)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if(user is null)
            {
                return null;
            }

            user.UserName = entity.UserName;
            user.Email = entity.Email;

            return user;
        }

        public async Task<string> GetUserAvatarAsync(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user is null)
            {
                return null;
            }

            return _containerClient.GetBlobClient(userId).Uri.AbsoluteUri;
        }

        public async Task<string> UpdateUserAvatarAsync(string userId, IFormFile image)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user is null)
            {
                return null;
            }

            BlobClient client = _containerClient.GetBlobClient(userId);

            await using (Stream? data = image.OpenReadStream())
            {
                await client.UploadAsync(data, overwrite: true);
            }

            return client.Uri.AbsoluteUri;
        }
    }
}
