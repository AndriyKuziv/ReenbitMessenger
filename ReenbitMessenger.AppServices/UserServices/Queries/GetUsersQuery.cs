using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.UserServices.Queries
{
    public class GetUsersQuery : IQuery<IEnumerable<IdentityUser>>
    {
        public int NumberOfUsers { get; }
        public int Page { get; }
        public string ValueContains { get; }
        public bool Ascending { get; }
        public string OrderBy { get; }

        public GetUsersQuery(int numberOfUsers = 20, string valueContains = "", int page = 0,
            bool ascending = true, string orderBy = "Username")
        {
            OrderBy = orderBy;
            NumberOfUsers = numberOfUsers;
            ValueContains = valueContains;
            Page = page;
            Ascending = ascending;
        }
    }
}
