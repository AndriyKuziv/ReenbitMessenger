using FluentValidation;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators
{
    public class AddUsersToGroupChatCommandValidator : AbstractValidator<AddUsersToGroupChatCommand>
    {
        public AddUsersToGroupChatCommandValidator(IGroupChatRepository groupChatRepository, IUserRepository userRepository)
        {
            RuleFor(cmd => cmd.GroupChatId).MustAsync(async (chatId, _) =>
            {
                return await groupChatRepository.GetAsync(chatId) != null;
            }).WithMessage("Group chat must exist.");

            RuleFor(cmd => cmd.UsersIds).NotEmpty().WithMessage("List of users to add cannot be empty.");
            RuleFor(cmd => cmd.UsersIds).MustAsync(async (usersIds, _) =>
            {
                foreach (var userId in usersIds)
                {
                    if (await userRepository.GetAsync(userId) is null) return false;
                }
                return true;
            }).WithMessage("All of the users ids must exist.");

            RuleFor(cmd => new { cmd.GroupChatId, cmd.UsersIds })
                .MustAsync(async (cmd, _) =>
                {
                    foreach (var userId in cmd.UsersIds)
                    {
                        if (await groupChatRepository.IsInGroupChat(cmd.GroupChatId, userId)) return false;
                    }
                    return true;
                }).WithMessage("All of the given users must not be members of this group chat.");
        }
    }
}
