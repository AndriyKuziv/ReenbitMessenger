using FluentValidation;
using FluentValidation.Results;

namespace ReenbitMessenger.DataAccess.Utils
{
    public sealed class ValidatorsHandler : IValidatorsHandler
    {
        private readonly IValidatorFactory _validatorFactory;
        private readonly Dictionary<Type, object> validators = new Dictionary<Type, object>();

        public ValidatorsHandler(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public async Task<ValidationResult> ValidateAsync<T>(T model)
        {
            var type = typeof(IValidator<T>);

            if (!validators.Keys.Contains(type))
            {
                validators[type] = _validatorFactory.GetValidator<T>();
            }

            return await ((IValidator<T>)validators[type]).ValidateAsync(model);
        }
    }
}
