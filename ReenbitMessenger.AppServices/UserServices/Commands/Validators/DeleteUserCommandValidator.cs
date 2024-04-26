using FluentValidation;
using ReenbitMessenger.AppServices.UserServices.Commands;

namespace ReenbitMessenger.AppServices.UserServices.Commands.Validators
{
    public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(delcomm => delcomm.UserId).NotEmpty();
        }
    }
}
