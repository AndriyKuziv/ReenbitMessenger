using FluentValidation;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators
{
    public sealed class EditGroupChatCommandValidator : AbstractValidator<EditGroupChatCommand>
    {
        public EditGroupChatCommandValidator(IGroupChatRepository groupChatRepository)
        {
            RuleFor(cmd => cmd.GroupChatId).MustAsync(async (chatId, _) =>
            {
                return await groupChatRepository.GetAsync(chatId) != null;
            }).WithMessage("Group chat must exist.");

            RuleFor(cmd => cmd.Name).NotEmpty();
        }
    }
}
