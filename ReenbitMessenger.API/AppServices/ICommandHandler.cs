namespace ReenbitMessenger.API.AppServices
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task<bool> Handle(TCommand command);
    }
}
