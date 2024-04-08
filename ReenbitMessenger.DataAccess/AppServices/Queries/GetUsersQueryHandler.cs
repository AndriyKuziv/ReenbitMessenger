using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<IdentityUser>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        public GetUsersQueryHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityUser>> Handle(GetUsersQuery query)
        {
            if (query.NumberOfUsers > 0)
            {
                return _userManager.Users
                .Where(usr =>
                    usr.UserName.Contains(query.UsernameContains) &&
                    usr.Email.Contains(query.EmailContains))
                .Skip(query.StartAt)
                .Take(query.NumberOfUsers)
                .ToList();
            }

            return _userManager.Users
                .Where(usr =>
                    usr.UserName.Contains(query.UsernameContains) &&
                    usr.Email.Contains(query.EmailContains))
                .Skip(query.StartAt)
                .ToList();
        }
    }
}
