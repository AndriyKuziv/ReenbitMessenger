using FluentValidation;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators
{
    public class SendMessageToGroupChatCommandValidator : AbstractValidator<SendMessageToGroupChatCommand>
    {
        public SendMessageToGroupChatCommandValidator(IGroupChatRepository groupChatRepository, IUserRepository userRepository)
        {
            RuleFor(cmd => cmd.GroupChatId).MustAsync(async (chatId, _) =>
            {
                return await groupChatRepository.GetAsync(chatId) != null;
            }).WithMessage("Group chat must exist.");

            RuleFor(cmd => cmd.UserId).MustAsync(async (usersId, _) =>
            {
                return await userRepository.GetAsync(usersId) != null;
            }).WithMessage("Sender user must exist.");

            RuleFor(cmd => cmd.Text).NotEmpty();
        }
    }
}
