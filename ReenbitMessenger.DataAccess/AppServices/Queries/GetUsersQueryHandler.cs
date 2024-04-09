using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Models.Domain;
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
            var userProp = typeof(IdentityUser).GetProperties().FirstOrDefault(prop => string.Equals(prop.Name, query.OrderBy,
                StringComparison.OrdinalIgnoreCase));

            if (userProp is null)
            {
                userProp = typeof(IdentityUser).GetProperty("UserName");
            }

            SortOrder sortOrder = query.SortOrder == "Descending" ? SortOrder.Descending : SortOrder.Ascending;

            if (query.NumberOfUsers <= 0)
            {
                return await _userRepository.FilterAsync(
                predicate: usr => usr.Email.Contains(query.ValueContains) ||
                    usr.UserName.Contains(query.ValueContains) ||
                    usr.Id.Contains(query.ValueContains),
                orderBy: usr => userProp.GetValue(usr),
                sortOrder: sortOrder
                );
            }

            return await _userRepository.FilterAsync(
                predicate: usr => usr.Email.Contains(query.ValueContains) ||
                    usr.UserName.Contains(query.ValueContains) ||
                    usr.Id.Contains(query.ValueContains),
                orderBy: usr => userProp.GetValue(usr),
                sortOrder: sortOrder,
                startAt: query.Page * query.NumberOfUsers,
                take: query.NumberOfUsers
                );
        }
    }
}
