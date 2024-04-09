using System.Data.SqlClient;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class GetUsersRequest
    {
        public int NumberOfUsers { get; set; }
        public int Page { get; set; }
        public string ValueContains { get; set; } = "";
        public string SortOrder { get; set; } = "Ascending";
        public string OrderBy { get; set; }
    }
}
