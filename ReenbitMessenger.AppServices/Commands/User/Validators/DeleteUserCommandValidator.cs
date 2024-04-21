using FluentValidation;

namespace ReenbitMessenger.AppServices.Commands.User.Validators
{
    public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(delcomm => delcomm.UserId).NotEmpty();
        }
    }
}
