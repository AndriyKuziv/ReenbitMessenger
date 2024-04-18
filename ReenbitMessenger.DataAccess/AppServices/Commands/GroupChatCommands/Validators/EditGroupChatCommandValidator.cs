using FluentValidation;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands.Validators
{
    public sealed class EditGroupChatCommandValidator : AbstractValidator<EditGroupChatCommand>
    {
        public EditGroupChatCommandValidator(IGroupChatRepository groupChatRepository)
        {
            RuleFor(cmd => cmd.GroupChatId).MustAsync(async (chatId, _) =>
            {
                return await groupChatRepository.GetAsync(chatId) != null;
            }).WithMessage("Group cmd must exist");

            RuleFor(cmd => cmd.Name).NotEmpty();
        }
    }
}
