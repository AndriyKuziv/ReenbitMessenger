using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class GetUsersQuery : IQuery<IEnumerable<IdentityUser>>
    {
        public int NumberOfUsers { get; }
        public string UsernameContains { get; }
        public string EmailContains { get; }
        public int StartAt { get; }

        public GetUsersQuery()
        {
            NumberOfUsers = 0;
            UsernameContains = "";
            EmailContains = "";
            StartAt = 0;
        }
        public GetUsersQuery(int numberOfUsers, string usernameContains, string emailContains, int startAt)
        {
            NumberOfUsers = numberOfUsers;
            UsernameContains = usernameContains;
            EmailContains = emailContains;
            StartAt = startAt;
        }
    }
}
