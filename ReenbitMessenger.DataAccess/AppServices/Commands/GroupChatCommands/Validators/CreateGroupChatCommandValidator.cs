using FluentValidation;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands.Validators
{
    public sealed class CreateGroupChatCommandValidator : AbstractValidator<CreateGroupChatCommand>
    {
        public CreateGroupChatCommandValidator(IUserRepository userRepository)
        {
            RuleFor(chat => chat.UserId).NotEmpty();
            RuleFor(chat => chat.UserId).MustAsync(async (userId, _) =>
            {
                return await userRepository.GetAsync(userId) != null;
            }).WithMessage("User must exist.");

            RuleFor(chat => chat.Name).NotEmpty();
        }
    }
}
