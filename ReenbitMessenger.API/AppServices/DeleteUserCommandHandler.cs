using ReenbitMessenger.API.Repositories;

namespace ReenbitMessenger.API.AppServices
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand command)
        {
            var user = await _userRepository.DeleteAsync(command.Id);

            if (user is null) return false;

            await _userRepository.SaveAsync();

            return true;
        }
    }
}
