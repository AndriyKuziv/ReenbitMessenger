using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.Queries.User
{
    public class GetUserByIdQuery : IQuery<IdentityUser>
    {
        public string Id { get; }

        public GetUserByIdQuery(string id)
        {
            Id = id;
        }
    }
}
