namespace ReenbitMessenger.API.AppServices
{
    public class DeleteUserCommand : ICommand
    {
        public Guid Id { get; }

        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }
    }
}
