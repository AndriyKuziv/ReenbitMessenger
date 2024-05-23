using Microsoft.AspNetCore.Http;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class UploadUserAvatarRequest
    {
        public IFormFile AvatarIcon { get; set; }
    }
}
