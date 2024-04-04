using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.DataAccess.AppServices.Commands
{
    public class EditUserInfoCommandHandler : ICommandHandler<EditUserInfoCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditUserInfoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(EditUserInfoCommand command)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = new User()
            {
                Username = command.Username,
                Email = command.Email
            };

            user = await userRepository.UpdateAsync(command.Id, user);

            if (user is null) return false;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
