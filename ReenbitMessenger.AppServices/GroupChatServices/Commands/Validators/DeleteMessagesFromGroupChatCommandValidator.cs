using FluentValidation;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;
using System.Security.Cryptography;

namespace ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators
{
    public class DeleteMessageFromGroupChatCommandValidator : AbstractValidator<DeleteMessageFromGroupChatCommand>
    {
        public DeleteMessageFromGroupChatCommandValidator(IGroupChatRepository groupChatRepository)
        {
            RuleFor(cmd => cmd.GroupChatId)
                .MustAsync(async (gcId, _) =>
                {
                    return await groupChatRepository.GetAsync(gcId) != null;
                }).WithMessage("Group chat must exist");

            RuleFor(cmd => cmd.MessageId)
                .MustAsync(async (msgId, _) =>
                {
                    return await groupChatRepository.GetMessageAsync(msgId) != null;
                }).WithMessage("The given message must exist.");

            RuleFor(cmd => cmd.UserId).NotEmpty();
        }
    }
}
