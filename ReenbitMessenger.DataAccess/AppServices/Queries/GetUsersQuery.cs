using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.AppServices.Queries
{
    public class GetUsersQuery : IQuery<IEnumerable<IdentityUser>>
    {
        public int NumberOfUsers { get; }
        public int Page { get; }
        public string ValueContains { get; }
        public string SortOrder { get; }
        public string OrderBy { get; }

        public GetUsersQuery(int numberOfUsers = 20, string valueContains = "", int page = 0,
            string sortOrder = "Ascending", string orderBy = "Username")
        {
            OrderBy = orderBy;
            NumberOfUsers = numberOfUsers;
            ValueContains = valueContains;
            Page = page;
            SortOrder = sortOrder;
        }
    }
}
