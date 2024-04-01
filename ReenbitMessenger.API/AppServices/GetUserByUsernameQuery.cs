using ReenbitMessenger.API.Models.DTO;

namespace ReenbitMessenger.API.AppServices
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
