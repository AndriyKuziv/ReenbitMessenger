using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Utils;
using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.User
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
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var user = new IdentityUser()
            {
                UserName = command.Username,
                Email = command.Email
            };

            user = await userRepository.UpdateAsync(command.UserId, user);

            if (user is null) return false;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
