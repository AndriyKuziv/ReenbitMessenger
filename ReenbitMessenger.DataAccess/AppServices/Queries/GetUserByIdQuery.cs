using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class GetUserByIdQuery : IQuery<IdentityUser>
    {
        public Guid Id { get; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
