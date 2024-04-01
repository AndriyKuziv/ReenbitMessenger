namespace ReenbitMessenger.API.Models.DTO
{
    public class AddUsersToGroupRequest
    {
        IEnumerable<Guid> Users { get; set; }
    }
}
