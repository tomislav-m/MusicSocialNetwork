using System;

namespace TicketingService.Models
{
    public class Ticket
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int Count { get; set; }
        public DateTime DateTimeBought { get; set; }
    }
}
