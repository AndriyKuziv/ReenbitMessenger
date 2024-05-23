using Microsoft.AspNetCore.Http;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class UploadUserAvatarRequest
    {
        public IFormFile Avatar { get; set; }
    }
}
