namespace Common.MessageContracts.Event.Commands
{
    public enum MarkEventType { GOING, INTERESTED }

    public class MarkEvent
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public MarkEventType MarkEventType { get; set; }
    }
}
