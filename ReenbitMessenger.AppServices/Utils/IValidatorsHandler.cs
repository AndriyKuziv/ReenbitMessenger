using FluentValidation.Results;

namespace ReenbitMessenger.AppServices.Utils
{
    public interface IValidatorsHandler
    {
        Task<ValidationResult> ValidateAsync<T>(T model);
    }
}
