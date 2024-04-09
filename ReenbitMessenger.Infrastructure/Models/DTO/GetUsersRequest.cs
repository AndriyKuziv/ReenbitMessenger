using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class GetUsersRequest
    {
        public int NumberOfUsers { get; set; }
        public string ValueContains { get; set; } = "";
        public int Page { get; set; }
        public string SortOrder { get; set; } = "";
    }
}
