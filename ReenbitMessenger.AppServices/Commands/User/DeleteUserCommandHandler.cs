using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Commands.User
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand command)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            var user = await userRepository.DeleteAsync(command.UserId);

            if (user is null) return false;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
