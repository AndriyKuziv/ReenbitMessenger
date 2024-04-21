using Microsoft.Extensions.DependencyInjection;

namespace ReenbitMessenger.AppServices.Utils
{
    public sealed class HandlersDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public HandlersDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type commandType = command.GetType();
            Type handlerType = type.MakeGenericType(commandType);

            using var scope = _serviceProvider.CreateScope();
            dynamic handler = scope.ServiceProvider.GetService(handlerType);
            dynamic result = await handler.Handle((dynamic)command);

            return result;
        }

        public async Task<T> Dispatch<T>(ICommand<T> command)
        {
            Type type = typeof(ICommandHandler<,>);
            Type[] typeArgs = { command.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            using var scope = _serviceProvider.CreateScope();
            dynamic handler = scope.ServiceProvider.GetService(handlerType);
            T result = await handler.Handle((dynamic)command);

            return result;
        }

        public async Task<T> Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            using var scope = _serviceProvider.CreateScope();
            dynamic handler = scope.ServiceProvider.GetService(handlerType);
            T result = await handler.Handle((dynamic)query);

            return result;
        }
    }
}
