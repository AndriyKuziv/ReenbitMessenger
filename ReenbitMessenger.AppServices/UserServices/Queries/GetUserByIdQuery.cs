using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.UserServices.Queries
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
