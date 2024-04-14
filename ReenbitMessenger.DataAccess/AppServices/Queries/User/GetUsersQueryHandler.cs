using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System.Data.SqlClient;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.User
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
            // remake predicate
            return (await _userRepository.FilterAsync(
                predicate: usr => usr.Email.Contains(query.ValueContains) ||
                    usr.UserName.Contains(query.ValueContains) ||
                    usr.Id.Contains(query.ValueContains),
                orderBy: query.OrderBy,
                ascending: query.Ascending,
                startAt: query.Page * query.NumberOfUsers,
                take: query.NumberOfUsers
                )).ToList();
        }
    }
}
