using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Queries.User
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<IdentityUser>>
    {
        private readonly IUserRepository _userRepository;
        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<IdentityUser>> Handle(GetUsersQuery query)
        {
            return (await _userRepository.FindAsync(
                searchValue: query.ValueContains,
                orderBy: query.OrderBy,
                ascending: query.Ascending,
                startAt: query.Page * query.NumberOfUsers,
                take: query.NumberOfUsers
                )).ToList();
        }
    }
}
