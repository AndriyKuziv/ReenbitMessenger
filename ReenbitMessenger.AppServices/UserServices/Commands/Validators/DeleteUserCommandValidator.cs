using FluentValidation;
using ReenbitMessenger.AppServices.UserServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.UserServices.Commands.Validators
{
    public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator(IUserRepository userRepository)
        {
            RuleFor(cmd => cmd.UserId).MustAsync(async (userId, _) =>
            {
                return await userRepository.GetAsync(userId) != null;
            }).WithMessage("User must exist.");
        }
    }
}
