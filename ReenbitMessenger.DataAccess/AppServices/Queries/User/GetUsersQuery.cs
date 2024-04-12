using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries.User
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
