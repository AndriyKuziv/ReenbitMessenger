using FluentValidation;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands.Validators
{
    public sealed class EditGroupChatCommandValidator : AbstractValidator<EditGroupChatCommand>
    {
        public EditGroupChatCommandValidator(IGroupChatRepository groupChatRepository)
        {
            RuleFor(cmd => cmd.GroupChatId).MustAsync(async (chatId, _) =>
            {
                return await groupChatRepository.GetAsync(chatId) != null;
            }).WithMessage("Group chat must exist");

            RuleFor(cmd => cmd.Name).NotEmpty();
        }
    }
}
