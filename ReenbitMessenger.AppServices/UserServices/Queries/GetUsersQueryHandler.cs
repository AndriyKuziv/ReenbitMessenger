using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.UserServices.Queries
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<IdentityUser>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IdentityUser>> Handle(GetUsersQuery query)
        {
            return (await _unitOfWork.GetRepository<IUserRepository>().FindAsync(
                searchValue: query.ValueContains,
                orderBy: query.OrderBy,
                ascending: query.Ascending,
                startAt: query.Page * query.NumberOfUsers,
                take: query.NumberOfUsers
                )).ToList();
        }
    }
}
