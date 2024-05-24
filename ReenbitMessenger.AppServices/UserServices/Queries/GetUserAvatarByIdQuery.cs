namespace ReenbitMessenger.AppServices.UserServices.Queries
{
    public class GetUserAvatarByIdQuery : IQuery<string>
    {
        public string UserId { get; }
        public GetUserAvatarByIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
