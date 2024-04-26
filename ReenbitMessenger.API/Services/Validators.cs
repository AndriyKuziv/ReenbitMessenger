using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.AppServices.UserServices.Commands.Validators;

namespace ReenbitMessenger.API.Services
{
    public static class Validators
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
        }
    }
}
