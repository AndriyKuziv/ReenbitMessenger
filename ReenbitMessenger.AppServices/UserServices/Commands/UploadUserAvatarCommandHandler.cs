using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace ReenbitMessenger.AppServices.UserServices.Commands
{
    public class UploadUserAvatarCommandHandler : ICommandHandler<UploadUserAvatarCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly int avatarMaxWidth = 300;
        private readonly int avatarMaxHeight = 300;

        public UploadUserAvatarCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UploadUserAvatarCommand command)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            var resizedImage = await ResizeImage(command.Avatar);

            var avatarUrl = await userRepository.UpdateUserAvatarAsync(command.UserId, resizedImage);

            await _unitOfWork.SaveAsync();

            return avatarUrl;
        }

        private async Task<IFormFile> ResizeImage(IFormFile imageFile)
        {
            var stream = new MemoryStream();
            await imageFile.CopyToAsync(stream);
            stream.Position = 0;

            using (var image = Image.Load(stream))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(avatarMaxWidth, avatarMaxHeight),
                    Mode = ResizeMode.Max
                }));

                stream.Position = 0;
                image.Save(stream, new PngEncoder());

                return new FormFile(stream, 0, stream.Length, imageFile.Name, imageFile.FileName);
            }
        }
    }
}
