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

        public GetUsersQuery()
        {
            NumberOfUsers = -1;
            UsernameContains = "";
            EmailContains = "";
        }
        public GetUsersQuery(int numberOfUsers, string usernameContains, string emailContains)
        {
            NumberOfUsers = numberOfUsers;
            UsernameContains = usernameContains;
            EmailContains = emailContains;
        }
    }
}
