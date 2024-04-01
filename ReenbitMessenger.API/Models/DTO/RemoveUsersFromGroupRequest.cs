namespace ReenbitMessenger.API.Models.DTO
{
    public class RemoveUsersFromGroupRequest
    {
        IEnumerable<Guid> Users { get; set; }
    }
}
