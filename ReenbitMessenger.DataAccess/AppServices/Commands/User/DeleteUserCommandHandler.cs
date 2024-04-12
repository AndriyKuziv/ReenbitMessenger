using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.User
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

            var user = await userRepository.DeleteAsync(command.Id);

            if (user is null) return false;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
