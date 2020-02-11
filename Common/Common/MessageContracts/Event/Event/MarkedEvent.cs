using Common.MessageContracts.Event.Commands;
using System;

namespace Common.MessageContracts.Event.Event
{
    public class MarkedEvent : IEvent
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public MarkEventType MarkEventType { get; set; }

        public string Type => nameof(MarkedEvent);

        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }
    }
}
