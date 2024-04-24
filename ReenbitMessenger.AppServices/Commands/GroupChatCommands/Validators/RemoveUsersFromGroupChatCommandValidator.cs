using FluentValidation;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands.Validators
{
    public class RemoveUsersFromGroupChatCommandValidator : AbstractValidator<RemoveUsersFromGroupChatCommand>
    {
        public RemoveUsersFromGroupChatCommandValidator(IUserRepository userRepository, IGroupChatRepository groupChatRepository)
        {
            RuleFor(cmd => cmd.GroupChatId)
                .MustAsync(async (gcId, _) =>
                {
                    return await groupChatRepository.GetAsync(gcId) != null;
                }).WithMessage("Group chat must exist.");

            RuleFor(cmd => cmd.UsersIds)
                .NotEmpty().WithMessage("List of users ids cannot be empty.")
                .MustAsync(async (usersIds, _) =>
                {
                    foreach (var userId in usersIds)
                    {
                        if (await userRepository.GetAsync(userId) is null)
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("All users must exist.");

            RuleFor(cmd => new { cmd.GroupChatId, cmd.UsersIds })
                .MustAsync(async (cmd, _) =>
                {
                    foreach (var userId in cmd.UsersIds)
                    {
                        if (!await groupChatRepository.IsInGroupChat(cmd.GroupChatId, userId)) return false;
                    }
                    return true;
                }).WithMessage("All of the given users must be members of this group chat.");
        }
    }
}
