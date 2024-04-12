using ReenbitMessenger.Infrastructure.Models;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Utils;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Security.Claims;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.User
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<bool> Handle(CreateUserCommand command)
        {
            var user = new IdentityUser()
            {
                UserName = command.Username,
                Email = command.Email,
            };

            var result = await _userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
            {
                return false;
            }

            //await _userManager.AddToRoleAsync(user, "user");

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
