using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class GetUserByUsernameQuery : IQuery<User>
    {
        public string Username { get; }

        public GetUserByUsernameQuery(string username)
        {
            Username = username;
        }
    }
}
