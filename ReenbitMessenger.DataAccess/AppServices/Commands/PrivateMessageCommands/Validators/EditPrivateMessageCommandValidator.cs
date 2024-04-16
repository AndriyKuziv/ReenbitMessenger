using FluentValidation;
using FluentValidation.Validators;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.PrivateMessageCommands.Validators
{
    public sealed class EditPrivateMessageCommandValidator : AbstractValidator<EditPrivateMessageCommand>
    {
        public EditPrivateMessageCommandValidator(IPrivateMessageRepository privateMessageRepository)
        {
            RuleFor(edcomm => edcomm.MessageId).GreaterThanOrEqualTo(0).WithMessage("Message Id cannot be lesser than 0");
            RuleFor(edcomm => edcomm.MessageId)
                .MustAsync(async (messageId, _) =>
                {
                    return await privateMessageRepository.GetAsync(messageId) != null;
                }).WithMessage("Message must exist.");

            RuleFor(edcomm => edcomm.Text).NotEmpty().WithMessage("New message text cannot be empty");
        }
    }
}
