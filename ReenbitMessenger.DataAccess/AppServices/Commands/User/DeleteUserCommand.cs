namespace ReenbitMessenger.DataAccess.AppServices.Commands.User
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
