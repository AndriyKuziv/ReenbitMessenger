using FluentValidation;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands.Validators
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
