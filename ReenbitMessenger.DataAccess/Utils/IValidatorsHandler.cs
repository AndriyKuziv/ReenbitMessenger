using FluentValidation.Results;

namespace ReenbitMessenger.DataAccess.Utils
{
    public interface IValidatorsHandler
    {
        Task<ValidationResult> ValidateAsync<T>(T model);
    }
}
