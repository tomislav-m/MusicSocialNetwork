using System;

namespace Common.MessageContracts.Ticketing.Events
{
    public class EventTicketAdded : IEvent
    {
        public int EventId { get; set; }
        public int TicketsOverall { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }

        public string Type => nameof(EventTicketAdded);

        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }
    }
}
