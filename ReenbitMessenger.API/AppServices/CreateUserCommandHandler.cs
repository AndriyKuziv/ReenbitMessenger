
using ReenbitMessenger.API.Models.DTO;
using ReenbitMessenger.API.Repositories;

namespace ReenbitMessenger.API.AppServices
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CreateUserCommand command)
        {
            var user = new Models.Domain.User()
            {
                Username = command.Username,
                Email = command.Email,
                Password = command.Password
            };

            user = await _userRepository.AddAsync(user);

            if (user is null) return false;

            await _userRepository.SaveAsync();

            return true;
        }
    }
}
