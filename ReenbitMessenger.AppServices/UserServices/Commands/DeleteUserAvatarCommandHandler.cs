using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.UserServices.Commands
{
    public class DeleteUserAvatarCommandHandler : ICommandHandler<DeleteUserAvatarCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserAvatarCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserAvatarCommand command)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            var deleteSuccess = await userRepository.DeleteUserAvatarAsync(command.UserId);

            if (!deleteSuccess)
            {
                return false;
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
