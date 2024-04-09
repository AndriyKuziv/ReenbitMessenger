using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System.Data.SqlClient;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
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
            if (query.NumberOfUsers <= 0)
            {
                return await _userRepository.FilterAsync(
                predicate: usr => usr.Email.Contains(query.ValueContains) ||
                    usr.UserName.Contains(query.ValueContains) ||
                    usr.Id.Contains(query.ValueContains),
                orderBy: usr => usr.UserName,
                query.SortOrder == "Descending" ? SortOrder.Descending : SortOrder.Ascending,
                query.Page
                );
            }
            
            return await _userRepository.FilterAsync(
                usr => usr.Email.Contains(query.ValueContains) ||
                    usr.UserName.Contains(query.ValueContains) ||
                    usr.Id.Contains(query.ValueContains),
                usr => usr.UserName,
                query.SortOrder == "Descending" ? SortOrder.Descending : SortOrder.Ascending,
                query.Page * query.NumberOfUsers,
                query.NumberOfUsers
                );
        }
    }
}
