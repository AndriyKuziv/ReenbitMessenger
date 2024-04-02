using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.DataAccess.AppServices
{
    public class EditUserInfoCommandHandler : ICommandHandler<EditUserInfoCommand>
    {
        private readonly IUserRepository _userRepository;

        public EditUserInfoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(EditUserInfoCommand command)
        {
            var user = new User()
            {
                Username = command.Username,
                Email = command.Email
            };

            user = await _userRepository.UpdateAsync(command.Id, user);

            if (user is null) return false;

            await _userRepository.SaveAsync();

            return true;
        }
    }
}
