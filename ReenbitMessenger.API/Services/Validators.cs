using FluentValidation;
using ReenbitMessenger.DataAccess.AppServices.Commands.User.Validators;

namespace ReenbitMessenger.API.Services
{
    public static class Validators
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
        }
    }
}
