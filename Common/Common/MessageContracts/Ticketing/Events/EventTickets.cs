using System;

namespace Common.MessageContracts.Ticketing.Events
{
    public class EventTickets : IEvent
    {
        public int EventId { get; set; }
        public int TicketsOverall { get; set; }
        public int TicketsSold { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }

        public string Type => nameof(EventTickets);
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }

        public EventTickets()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
