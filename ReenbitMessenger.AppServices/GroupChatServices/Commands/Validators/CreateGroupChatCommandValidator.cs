using FluentValidation;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators
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
