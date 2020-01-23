using System;

namespace Common.MessageContracts.Ticketing.Events
{
    public class TicketBought : IEvent
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public int Count { get; set; }

        public string Type => nameof(TicketBought);
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public TicketBought()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
