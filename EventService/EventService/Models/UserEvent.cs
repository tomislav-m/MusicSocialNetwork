using Common.MessageContracts.Event.Commands;

namespace EventService.Models
{
    public class UserEvent
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public MarkEventType MarkEventType { get; set; }
    }
}
