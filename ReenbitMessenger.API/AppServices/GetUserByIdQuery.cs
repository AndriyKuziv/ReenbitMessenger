using ReenbitMessenger.API.Models.DTO;

namespace ReenbitMessenger.API.AppServices
{
    public class GetUserByIdQuery : IQuery<User>
    {
        public Guid Id { get; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
