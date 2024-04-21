using FluentValidation;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.AppServices.Commands.GroupChatCommands.Validators
{
    public class SendMessageToGroupChatCommandValidator : AbstractValidator<SendMessageToGroupChatCommand>
    {
        public SendMessageToGroupChatCommandValidator()
        {
            RuleFor(cmd => cmd.UserId).NotEmpty();
            RuleFor(cmd => cmd.Text).NotEmpty().WithMessage("Message cannot be empty");
        }
    }
}
