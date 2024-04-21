using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.Queries.User
{
    public class GetUserByUsernameQueryHandler :
        IQueryHandler<GetUserByUsernameQuery, IdentityUser>
    {
        private readonly UserManager<IdentityUser> _userManager;
        public GetUserByUsernameQueryHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser> Handle(GetUserByUsernameQuery query)
        {
            var user = await _userManager.FindByNameAsync(query.Username);

            return user;
        }
    }
}
