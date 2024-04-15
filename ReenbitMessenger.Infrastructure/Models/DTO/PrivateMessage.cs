
namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class PrivateMessage
    {
        public long Id { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string Text { get; set; }
        public DateTime SentTime { get; set; }
        public long? MessageToReplyId { get; set; }

        // Navigation properties
        public User? SenderUser { get; set; }
        public User? ReceiverUser { get; set; }
    }
}
