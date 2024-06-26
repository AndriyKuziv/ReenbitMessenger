﻿using FluentValidation;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.PrivateMessageServices.Commands.Validators
{
    public sealed class DeletePrivateMessageCommandValidator : AbstractValidator<DeletePrivateMessageCommand>
    {
        public DeletePrivateMessageCommandValidator(IPrivateMessageRepository privateMessageRepository)
        {
            RuleFor(delcomm => delcomm.MessageId).GreaterThanOrEqualTo(0).WithMessage("Message Id cannot be lesser than 0.");
            RuleFor(delcomm => delcomm.MessageId).MustAsync(async (messageId, _) =>
            {
                return await privateMessageRepository.GetAsync(messageId) != null;
            }).WithMessage("Message must exist.");
        }
    }
}
