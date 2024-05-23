using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.UserServices.Commands
{
    public class UploadUserAvatarCommandHandler : ICommandHandler<UploadUserAvatarCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UploadUserAvatarCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UploadUserAvatarCommand command)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            return await userRepository.UpdateUserAvatarAsync(command.UserId, command.Avatar);
        }
    }
}
