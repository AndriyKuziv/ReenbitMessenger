using ReenbitMessenger.Infrastructure.Models;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.DataAccess.AppServices.Commands
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateUserCommand command)
        {
            var userRepository = _unitOfWork.GetRepository<User>();

            var user = new User()
            {
                Username = command.Username,
                Email = command.Email,
                Password = command.Password
            };

            user = await userRepository.AddAsync(user);

            if (user is null) return false;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
