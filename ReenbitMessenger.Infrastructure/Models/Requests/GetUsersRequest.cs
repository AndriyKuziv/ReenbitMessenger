using System.Data.SqlClient;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class GetUsersRequest
    {
        public int NumberOfUsers { get; set; }
        public int Page { get; set; }
        public string ValueContains { get; set; } = "";
        public bool Ascending { get; set; } = true;
        public string OrderBy { get; set; }
    }
}
