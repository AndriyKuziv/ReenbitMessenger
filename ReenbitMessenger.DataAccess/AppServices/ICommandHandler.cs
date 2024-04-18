namespace ReenbitMessenger.DataAccess.AppServices
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task<bool> Handle(TCommand command);
    }

    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command);
    }
}
