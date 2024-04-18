﻿using FluentValidation;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands.Validators
{
    public class DeleteMessagesFromGroupChatCommandValidator : AbstractValidator<DeleteMessagesFromGroupChatCommand>
    {
        public DeleteMessagesFromGroupChatCommandValidator(IGroupChatRepository groupChatRepository)
        {
            RuleFor(cmd => cmd.GroupChatId)
                .NotNull().WithMessage("Group chat id cannot be null.")
                .NotEmpty().WithMessage("Group chat id cannot be empty.")
                .MustAsync(async (gcId, _) =>
                {
                    return await groupChatRepository.GetAsync(gcId) != null;
                }).WithMessage("Group chat must exist");

            RuleFor(cmd => cmd.MessagesIds)
                .NotEmpty().WithMessage("List of given messages cannot be empty.")
                .MustAsync(async (msgsIds, _) =>
                {
                    foreach(var msgId in msgsIds)
                    {
                        if (await groupChatRepository.GetMessageAsync(msgId) is null)
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("All of the given messages must exist.");
        }
    }
}
