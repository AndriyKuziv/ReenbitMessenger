using System.Text.Json.Serialization;

namespace ReenbitMessenger.Infrastructure.Models.Requests
{
    public class CreateUserRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
