using FluentValidation;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;
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
            RuleFor(cmd => cmd.UserId).NotEmpty();
            RuleFor(cmd => cmd.UserId).MustAsync(async (userId, _) =>
            {
                return await userRepository.GetAsync(userId) != null;
            }).WithMessage("User must exist.");

            RuleFor(cmd => cmd.Name).NotEmpty();
        }
    }
}
