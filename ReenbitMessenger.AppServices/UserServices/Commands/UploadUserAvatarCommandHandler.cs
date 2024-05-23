using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.UserServices.Commands
{
    public class UploadUserAvatarCommandHandler : ICommandHandler<UploadUserAvatarCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public UploadUserAvatarCommandHandler(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public Task<bool> Handle(UploadUserAvatarCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
