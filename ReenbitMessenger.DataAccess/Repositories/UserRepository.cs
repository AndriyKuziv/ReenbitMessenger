using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using ReenbitMessenger.DataAccess.Models.Domain;

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

            var usrAvatar = await _dbContext.UserAvatar.FirstOrDefaultAsync(ua => ua.UserId == userId);

            if (usrAvatar is null)
            {
                return null;
            }

            return usrAvatar.AvatarUrl;
        }

        public async Task<string> UpdateUserAvatarAsync(string userId, IFormFile imageFile)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user is null)
            {
                return null;
            }

            BlobClient client = _containerClient.GetBlobClient(userId + Path.GetExtension(imageFile.FileName));

            await using (Stream data = imageFile.OpenReadStream())
            {
                await client.UploadAsync(data, overwrite: true);
            }

            UserAvatar userAvatar = await _dbContext.UserAvatar.FirstOrDefaultAsync(ua => ua.UserId == userId);

            if (userAvatar is null)
            {
                userAvatar = new UserAvatar()
                {
                    AvatarUrl = client.Uri.AbsoluteUri,
                    UserId = userId
                };

                await _dbContext.UserAvatar.AddAsync(userAvatar);
            }
            else
            {
                userAvatar.AvatarUrl = client.Uri.AbsoluteUri;
            }

            return client.Uri.AbsoluteUri;
        }
    }
}
