using ReenbitMessenger.API.AppServices;

namespace ReenbitMessenger.API.Utils
{
    public sealed class HandlersDispatcher
    {
        private readonly IServiceProvider _provider;

        public HandlersDispatcher(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }

        public async Task<bool> Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type commandType = command.GetType();
            Type handlerType = type.MakeGenericType(commandType);

            dynamic handler = _provider.GetService(handlerType);
            dynamic result = await handler.Handle((dynamic)command);

            return result;
        }

        public async Task<T> Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            T result = await handler.Handle((dynamic)query);

            return result;
        }
    }
}
