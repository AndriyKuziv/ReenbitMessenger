using FluentValidation;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Commands.PrivateMessageCommands.Validators
{
    public sealed class SendPrivateMessageCommandValidator : AbstractValidator<SendPrivateMessageCommand>
    {
        public SendPrivateMessageCommandValidator(IUserRepository userRepository,
            IPrivateMessageRepository privateMessageRepository)
        {
            RuleFor(scomm => scomm.SenderUserId)
                .NotEmpty().WithMessage("Sender id cannot be empty.");
            RuleFor(scomm => scomm.SenderUserId).MustAsync(async (senderId, _) =>
            {
                return await userRepository.GetAsync(senderId) != null;
            }).WithMessage("Sender user must exist.");


            RuleFor(scomm => scomm.ReceiverUserId)
                .NotEmpty().WithMessage("Receiver id cannot be empty.");
            RuleFor(scomm => scomm.ReceiverUserId).MustAsync(async (receiverId, _) =>
                {
                    return await userRepository.GetAsync(receiverId) != null;
                }).WithMessage("Receiver user must exist.");

            RuleFor(scomm => scomm.Text)
                .NotEmpty().WithMessage("Message text cannot be empty.");

            RuleFor(scomm => scomm.MessageToReplyId)
                .GreaterThanOrEqualTo(0).WithMessage("Message id cannot be less than 0");
            RuleFor(scomm => scomm.MessageToReplyId)
                .MustAsync(async (messageId, _) =>
                {
                    return messageId is null || await privateMessageRepository.GetAsync((long)messageId) != null;
                });
        }
    }
}
