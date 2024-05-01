using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.UserServices.Commands
{
    public class EditUserInfoCommandHandler : ICommandHandler<EditUserInfoCommand, IdentityUser>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditUserInfoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityUser> Handle(EditUserInfoCommand command)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var user = new IdentityUser()
            {
                UserName = command.Username,
                Email = command.Email
            };

            user = await userRepository.UpdateAsync(command.UserId, user);

            if (user is null)
            {
                return null;
            }

            await _unitOfWork.SaveAsync();

            return user;
        }
    }
}
