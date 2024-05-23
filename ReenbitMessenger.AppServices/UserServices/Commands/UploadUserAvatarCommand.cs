using Microsoft.AspNetCore.Http;

namespace ReenbitMessenger.AppServices.UserServices.Commands
{
    public class UploadUserAvatarCommand : ICommand<string>
    {
        public string UserId { get; }
        public IFormFile Avatar { get; }

        public UploadUserAvatarCommand(string userId, IFormFile avatar)
        {
            UserId = userId;
            Avatar = avatar;
        }
    }
}
