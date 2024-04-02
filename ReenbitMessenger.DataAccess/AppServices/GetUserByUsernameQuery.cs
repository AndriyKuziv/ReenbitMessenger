using ReenbitMessenger.Library.Models.DTO;

namespace ReenbitMessenger.DataAccess.AppServices
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
